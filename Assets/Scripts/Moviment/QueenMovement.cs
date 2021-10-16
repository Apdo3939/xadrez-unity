using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovement : Moviment
{
    public QueenMovement(bool maxTeam)
    {
        value = 900;
        if (maxTeam)
        {
            positionValue = AIController.instance.squareTable.queenGold;
        }
        else
        {
            positionValue = AIController.instance.squareTable.queenGreen;
        }
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();

        int limit = 99;

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, limit));

        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, limit));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, limit));

        return moves;
    }
}
