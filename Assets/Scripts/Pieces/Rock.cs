using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Piece
{
    private void Awake()
    {
        moviment = new RockMovement();
    }
    public override AffectedPiece CreatedAffected()
    {
        AffectedPieceKingRock aff = new AffectedPieceKingRock();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
