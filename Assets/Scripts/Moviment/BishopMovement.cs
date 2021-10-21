using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Moviment
{
    public BishopMovement(bool maxTeam)
    {
        value = 300;
        if (maxTeam)
        {
            positionValue = AIController.instance.squareTable.bishopGold;
        }
        else
        {
            positionValue = AIController.instance.squareTable.bishopGreen;
        }
    }
    public override List<AvailableMoves> GetValidMoves()
    {
        List<AvailableMoves> moves = new List<AvailableMoves>();

        int limit = 99;

        UntilBlockedPath(moves, new Vector2Int(1, 1), true, limit);
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, limit);

        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, limit);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, limit);

        return moves;
    }
}
