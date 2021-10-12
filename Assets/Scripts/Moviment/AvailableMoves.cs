using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AvailableMoves
{
    public Vector2Int pos;
    public MoveType moveType;
    public AvailableMoves(Vector2Int rcvPos, MoveType rcvMoveType)
    {
        pos = rcvPos;
        moveType = rcvMoveType;
    }
    public AvailableMoves(Vector2Int rcvPos)
    {
        pos = rcvPos;
        moveType = MoveType.Normal;
    }
}
