using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moviment
{
    public abstract List<AvailableMoves> GetValidMoves();
    public int value;
    public Dictionary<Vector2Int, int> positionValue;

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

    protected void UntilBlockedPath(List<AvailableMoves> moves, Vector2Int direction, bool includeBlocked, int limit)
    {
        Tile current = Board.instance.selectedPiece.tile;
        int currentCount = moves.Count;

        while (current != null && moves.Count < limit + currentCount)
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
                        moves.Insert(0, new AvailableMoves(current.pos));
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
        }
        return;
    }
}
