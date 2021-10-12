using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moviment
{
    public abstract List<AvailableMoves> GetValidMoves();
    public int value;

    protected bool IsEnemy(Tile tile)
    {
        if (tile.content != null && tile.content.transform.parent != Board.instance.selectedPiece.transform.parent)
        {
            return true;
        }
        return false;
    }

    protected Tile GetTile(Vector2Int position)
    {
        Tile tile;
        Board.instance.tiles.TryGetValue(position, out tile);
        return tile;
    }

    protected List<AvailableMoves> UntilBlockedPath(Vector2Int direction, bool includeBlocked, int limit)
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();
        Tile current = Board.instance.selectedPiece.tile;
        while (current != null && moves.Count < limit)
        {
            if (Board.instance.tiles.TryGetValue(current.pos + direction, out current))
            {
                if (current.content == null)
                {
                    moves.Add(new AvailableMoves(current.pos));
                }
                else if (IsEnemy(current))
                {
                    if (includeBlocked)
                    {
                        moves.Add(new AvailableMoves(current.pos));
                    }
                    return moves;
                }
                else
                {
                    return moves;
                }
            }
        }
        return moves;
    }
}
