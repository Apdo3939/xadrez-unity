using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Moviment savedMovement;
    public Moviment queenMovement;
    public Moviment knightMovement;
    protected override void Start()
    {
        base.Start();
        moviment = savedMovement = new PawnMoviment(maxTeam);
        queenMovement = new QueenMovement(maxTeam);
        knightMovement = new KnightMovement(maxTeam);
    }
    public override AffectedPiece CreatedAffected()
    {
        AffectedPiecePawn aff = new AffectedPiecePawn();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
