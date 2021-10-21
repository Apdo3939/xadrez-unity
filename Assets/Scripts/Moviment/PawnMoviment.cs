using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMoviment : Moviment
{
    Vector2Int direction;
    int promotionHeight = 0;
    public PawnMoviment(bool maxTeam)
    {
        value = 100;
        if (maxTeam)
        {
            direction = new Vector2Int(0, 1);
            promotionHeight = 7;
            positionValue = AIController.instance.squareTable.pawnGold;
        }
        else
        {
            direction = new Vector2Int(0, -1);
            promotionHeight = 0;
            positionValue = AIController.instance.squareTable.pawnGreen;
        }

    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moveable = new List<AvailableMoves>();
        List<AvailableMoves> moves = new List<AvailableMoves>();

        GetPawnAttack(moveable);

        if (!Board.instance.selectedPiece.wasMoved)
        {
            UntilBlockedPath(moves, direction, false, 2);
            if (moves.Count == 2)
            {
                moves[1] = new AvailableMoves(moves[1].pos, MoveType.PawnDoubleMove);
            }
        }
        else
        {
            UntilBlockedPath(moves, direction, false, 1);
            if (moves.Count > 0)
            {
                moves[0] = CheckPromotion(moves[0]);
            }

        }
        moveable.AddRange(moves);
        return moveable;
    }
    void GetPawnAttack(List<AvailableMoves> pawnAttack)
    {
        Piece piece = Board.instance.selectedPiece;
        Vector2Int leftPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + direction.y);
        Vector2Int rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + direction.y);

        GetPawnAttack(GetTile(leftPos), pawnAttack);
        GetPawnAttack(GetTile(rightPos), pawnAttack);
    }

    void GetPawnAttack(Tile tile, List<AvailableMoves> pawnAttack)
    {
        if (tile == null)
        {
            return;
        }
        if (IsEnemy(tile))
        {
            pawnAttack.Add(new AvailableMoves(tile.pos, MoveType.Normal));
        }
        else if (PieceMovementState.enPassantFlags.moveType == MoveType.EnPassant && PieceMovementState.enPassantFlags.pos == tile.pos)
        {
            pawnAttack.Add(new AvailableMoves(tile.pos, MoveType.EnPassant));
        }
    }

    AvailableMoves CheckPromotion(AvailableMoves availableMove)
    {
        if (availableMove.pos.y != promotionHeight)
        {
            return availableMove;
        }
        return new AvailableMoves(availableMove.pos, MoveType.Promotion);
    }
}
