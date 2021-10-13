using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private void Awake()
    {
        moviment = new KingMoviment();
    }

    public override AffectedPiece CreatedAffected()
    {
        AffectedPieceKingRock aff = new AffectedPieceKingRock();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
