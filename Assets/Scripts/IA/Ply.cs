using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply
{
    public int score;
    public List<AffectedPiece> changes;
    public Ply originPly;
    public Ply bestFuture;
    //public List<Ply> futurePlies;
    public AvailableMoves enPassantFlags;
}
