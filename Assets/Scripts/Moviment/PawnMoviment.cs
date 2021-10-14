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
        }
        else
        {
            direction = new Vector2Int(0, -1);
            promotionHeight = 0;
        }

    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moveable = GetPawnAttack(direction);
        List<AvailableMoves> moves;

        if (!Board.instance.selectedPiece.wasMoved)
        {
            moves = UntilBlockedPath(direction, false, 2);
            if (moves.Count == 2)
            {
                moves[1] = new AvailableMoves(moves[1].pos, MoveType.PawnDoubleMove);
            }
        }
        else
        {
            moves = UntilBlockedPath(direction, false, 1);
            if (moves.Count > 0)
            {
                moves[0] = CheckPromotion(moves[0]);
            }

        }
        moveable.AddRange(moves);
        return moveable;
    }
    List<AvailableMoves> GetPawnAttack(Vector2Int direction)
    {
        List<AvailableMoves> pawnAttack = new List<AvailableMoves>();
        Piece piece = Board.instance.selectedPiece;
        Vector2Int leftPos = new Vector2Int(piece.tile.pos.x - 1, piece.tile.pos.y + direction.y);
        Vector2Int rightPos = new Vector2Int(piece.tile.pos.x + 1, piece.tile.pos.y + direction.y);

        GetPawnAttack(GetTile(leftPos), pawnAttack);
        GetPawnAttack(GetTile(rightPos), pawnAttack);

        return pawnAttack;
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
        int promotionHeight = 0;
        if (Board.instance.selectedPiece.maxTeam)
        {
            promotionHeight = 7;
        }
        if (availableMove.pos.y != promotionHeight)
        {
            return availableMove;
        }
        return new AvailableMoves(availableMove.pos, MoveType.Promotion);
    }
}
