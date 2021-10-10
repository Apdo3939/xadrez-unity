using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMoviment : Moviment
{
    public KingMoviment()
    {
        value = 10000;
    }
    public override List<Tile> GetValidMoves()
    {
        List<Tile> moves = new List<Tile>();

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 1));

        SetNormalMove(moves);

        moves.AddRange(Castling());
        return moves;
    }

    List<Tile> Castling()
    {
        List<Tile> moves = new List<Tile>();
        if (Board.instance.selectedPiece.wasMoved)
        {
            return moves;
        }
        Tile temp = CheckRock(new Vector2Int(1, 0));
        if (temp != null)
        {
            temp.moveType = MoveType.Castling;
            moves.Add(temp);
        }
        temp = CheckRock(new Vector2Int(-1, 0));
        if (temp != null)
        {
            temp.moveType = MoveType.Castling;
            moves.Add(temp);
        }
        return moves;
    }

    Tile CheckRock(Vector2Int direction)
    {
        Rock rock;
        Tile currentTile = GetTile(Board.instance.selectedPiece.tile.pos + direction);
        while (currentTile != null)
        {
            if (currentTile.content != null)
            {
                break;
            }
            currentTile = GetTile(currentTile.pos + direction);
        }
        if (currentTile == null)
        {
            return null;
        }
        rock = currentTile.content as Rock;
        if (rock == null || rock.wasMoved)
        {
            return null;
        }
        return rock.tile;
    }
}
