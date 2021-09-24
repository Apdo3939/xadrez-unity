using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public override void Enter()
    {
        Debug.Log("Piece Moved...");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
