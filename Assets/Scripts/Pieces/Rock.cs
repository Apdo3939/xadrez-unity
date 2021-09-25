using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Piece
{
    private void Awake()
    {
        moviment = new RockMovement();
    }
}
