using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    protected override void Start()
    {
        base.Start();
        moviment = new QueenMovement(maxTeam);
    }
}
