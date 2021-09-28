using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PieceSelectionState : State
{
    public override void Enter()
    {
        InputController.instance.tileClicked += PieceClicked;
        SetColliders(true);
    }

    public override void Exit()
    {
        InputController.instance.tileClicked -= PieceClicked;
        SetColliders(false);
    }

    void PieceClicked(object sender, object args)
    {
        Piece piece = sender as Piece;
        Player player = args as Player;
        if (machine.currentlyPlaying == player)
        {
            Debug.Log("Select piece clicked..." + player);
            Board.instance.selectedPiece = piece;
            machine.ChangeTo<MovieSelectionState>();
        }

    }

    void SetColliders(bool state)
    {
        foreach (BoxCollider2D b in machine.currentlyPlaying.GetComponentsInChildren<BoxCollider2D>())
        {
            b.enabled = state;
        }
    }
}
