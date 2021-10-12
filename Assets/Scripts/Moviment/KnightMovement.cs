using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Moviment
{
    public KnightMovement()
    {
        value = 300;
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();
        moves.AddRange(GetMoveKnight1(new Vector2Int(0, 1)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(0, -1)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(1, 0)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(-1, 0)));

        return moves;
    }

    List<AvailableMoves> GetMoveKnight1(Vector2Int direction)
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();
        Tile current = Board.instance.selectedPiece.tile;
        Tile temp = GetTile(current.pos + direction * 2);
        if (temp != null)
        {
            moves.AddRange(GetMoveKnight2(temp.pos, new Vector2Int(direction.y, direction.x)));
        }
        return moves;
    }

    List<AvailableMoves> GetMoveKnight2(Vector2Int pos, Vector2Int direction)
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();
        Tile tile1 = GetTile(pos + direction);
        Tile tile2 = GetTile(pos - direction);
        if (tile1 != null && (tile1.content == null || IsEnemy(tile1)))
        {
            moves.Add(new AvailableMoves(tile1.pos));
        }
        if (tile2 != null && (tile2.content == null || IsEnemy(tile2)))
        {
            moves.Add(new AvailableMoves(tile2.pos));
        }
        return moves;
    }
}
