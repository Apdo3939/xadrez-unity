using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : Moviment
{
    public RockMovement()
    {
        value = 500;
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();

        int limit = 99;

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, limit));

        return moves;
    }
}
