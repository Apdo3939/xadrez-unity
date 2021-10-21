using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : Moviment
{
    public RockMovement(bool maxTeam)
    {
        value = 500;
        if (maxTeam)
        {
            positionValue = AIController.instance.squareTable.rockGold;
        }
        else
        {
            positionValue = AIController.instance.squareTable.rockGreen;
        }
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();

        int limit = 99;

        UntilBlockedPath(moves, new Vector2Int(1, 0), true, limit);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, limit);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, limit);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, limit);

        return moves;
    }
}
