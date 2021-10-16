using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Piece
{
    protected override void Start()
    {
        base.Start();
        moviment = new RockMovement(maxTeam);
    }
    public override AffectedPiece CreatedAffected()
    {
        AffectedPieceKingRock aff = new AffectedPieceKingRock();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
