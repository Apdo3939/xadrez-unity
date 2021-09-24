using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public override async void Enter()
    {
        Debug.Log("Piece Moved...");
        Piece piece = Board.instance.selectedPiece;
        piece.transform.position = Board.instance.selectedHighlight.transform.position;
        piece.tile.content = null;
        piece.tile = Board.instance.selectedHighlight.tile;

        if (piece.tile.content != null)
        {
            Piece deadPiece = piece.tile.content;
            Debug.Log("Piece {0}... removida", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }

        piece.tile.content = piece;

        await Task.Delay(100);
        machine.ChangeTo<TurnEndState>();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
