using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    protected override void Start()
    {
        base.Start();
        moviment = new KnightMovement(maxTeam);
    }
}
