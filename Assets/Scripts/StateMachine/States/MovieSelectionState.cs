using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("Select piece clicked moved...");
        List<Tile> moves = Board.instance.selectedPiece.moviment.GetValidMoves();
        foreach (Tile t in moves)
        {
            Debug.Log(t.pos);
        }
    }

    public override void Exit()
    {

    }
}
