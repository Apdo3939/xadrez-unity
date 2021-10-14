using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public Moviment savedMovement;
    public Moviment queenMovement = new QueenMovement();
    public Moviment knightMovement = new KnightMovement();
    protected override void Start()
    {
        base.Start();
        moviment = savedMovement = new PawnMoviment(maxTeam);
    }
    public override AffectedPiece CreatedAffected()
    {
        AffectedPiecePawn aff = new AffectedPiecePawn();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
