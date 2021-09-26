using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Moviment
{
    public override List<Tile> GetValidMoves()
    {
        List<Tile> moves = new List<Tile>();
        moves.AddRange(GetMoveKnight1(new Vector2Int(0, 1)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(0, -1)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(1, 0)));
        moves.AddRange(GetMoveKnight1(new Vector2Int(-1, 0)));
        return moves;
    }

    List<Tile> GetMoveKnight1(Vector2Int direction)
    {
        List<Tile> moves = new List<Tile>();
        Tile current = Board.instance.selectedPiece.tile;
        Tile temp = GetTile(current.pos + direction * 2);
        if (temp != null)
        {
            moves.AddRange(GetMoveKnight2(temp.pos, new Vector2Int(direction.y, direction.x)));
        }
        return moves;
    }

    List<Tile> GetMoveKnight2(Vector2Int pos, Vector2Int direction)
    {
        List<Tile> moves = new List<Tile>();
        Tile tile1 = GetTile(pos + direction);
        Tile tile2 = GetTile(pos - direction);
        if (tile1 != null && (tile1.content == null || IsEnemy(tile1)))
        {
            moves.Add(tile1);
        }
        if (tile2 != null && (tile2.content == null || IsEnemy(tile2)))
        {
            moves.Add(tile2);
        }
        return moves;
    }
}
