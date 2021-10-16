using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    protected override void Start()
    {
        base.Start();
        moviment = new KingMoviment(maxTeam);
    }

    public override AffectedPiece CreatedAffected()
    {
        AffectedPieceKingRock aff = new AffectedPieceKingRock();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
