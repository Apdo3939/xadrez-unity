using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Moviment
{
    public BishopMovement()
    {
        value = 300;
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();

        int limit = 99;

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, limit));

        return moves;
    }
}
