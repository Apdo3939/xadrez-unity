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
        moviment = savedMovement = new PawnMoviment(GetDirection());
    }
    public override AffectedPiece CreatedAffected()
    {
        AffectedPiecePawn aff = new AffectedPiecePawn();
        aff.wasMoved = wasMoved;
        return aff;
    }
    Vector2Int GetDirection()
    {
        if (maxTeam)
        {
            return new Vector2Int(0, 1);
        }
        return new Vector2Int(0, -1);
    }
}
