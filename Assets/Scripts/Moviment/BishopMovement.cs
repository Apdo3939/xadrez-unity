using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Moviment
{
    public BishopMovement()
    {
        value = 300;
    }
    public override List<Tile> GetValidMoves()
    {
        List<Tile> moves = new List<Tile>();

        int limit = 99;

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, limit));

        SetNormalMove(moves);

        return moves;
    }
}
