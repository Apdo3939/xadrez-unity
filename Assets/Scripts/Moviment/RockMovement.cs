using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : Moviment
{
    public RockMovement()
    {
        value = 5;
    }
    public override List<Tile> GetValidMoves()
    {
        List<Tile> moves = new List<Tile>();

        int limit = 99;

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, limit));
        SetNormalMove(moves);
        return moves;
    }
}
