using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;
    public Ply currentState;
    public HighlightsClick AIhighlight;
    public int objectivePlyDepth = 2;
    int calculationCount;
    float lastInterval;
    public AvailableMoves enPassantSaved;
    public PieceSquareTable squareTable = new PieceSquareTable();
    Ply minPly;
    Ply maxPly;

    void Awake()
    {
        instance = this;
        maxPly = new Ply();
        maxPly.score = 999999;
        minPly = new Ply();
        minPly.score = -999999;
        squareTable.SetDictionaries();
    }

    [ContextMenu("Calculate Plays")]
    public async Task<Ply> CalculatePlays()
    {
        lastInterval = Time.realtimeSinceStartup;
        int minimaxDirection;

        if (StateMachineController.instance.currentlyPlaying == StateMachineController.instance.player1)
        {
            minimaxDirection = 1;
        }
        else
        {
            minimaxDirection = -1;
        }

        enPassantSaved = PieceMovementState.enPassantFlags;
        Ply currentPly = CreateSnapShot();
        calculationCount = 0;

        currentPly.originPly = null;
        int currentPlyDepth = 0;
        currentPly.changes = new List<AffectedPiece>();

        Task<Ply> calculation = CalculatePly(currentPly,
            GetTeam(currentPly, minimaxDirection),
            currentPlyDepth, minimaxDirection);
        await calculation;
        currentPly.bestFuture = calculation.Result;

        Debug.Log("Calculations: " + calculationCount);
        Debug.Log("Time: " + (Time.realtimeSinceStartup - lastInterval));
        PrintBestPly(currentPly.bestFuture);
        PieceMovementState.enPassantFlags = enPassantSaved;
        return currentPly.bestFuture;
    }

    List<PieceEvaluation> GetTeam(Ply ply, int minimaxDirection)
    {
        if (minimaxDirection == 1)
        {
            return ply.golds;
        }
        else
        {
            return ply.greens;
        }
    }

    async Task<Ply> CalculatePly(Ply parentPly, List<PieceEvaluation> team, int currentPlyDepth, int minimaxDirection)
    {
        parentPly.futurePlies = new List<Ply>();
        currentPlyDepth++;
        if (currentPlyDepth > objectivePlyDepth)
        {
            EvaluateBoard(parentPly);
            return parentPly;
        }
        if (minimaxDirection == 1)
        {
            parentPly.bestFuture = minPly;
        }
        else
        {
            parentPly.bestFuture = maxPly;
        }

        foreach (PieceEvaluation eva in team)
        {
            foreach (AvailableMoves move in eva.availableMoves)
            {
                calculationCount++;
                Board.instance.selectedPiece = eva.piece;
                Board.instance.selectedMove = move;
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, move.moveType);

                await tcs.Task;
                Ply newPly = CreateSnapShot(parentPly);
                newPly.changes = PieceMovementState.changes;
                newPly.enPassantFlags = PieceMovementState.enPassantFlags;

                List<PieceEvaluation> nextTeam = GetTeam(newPly, minimaxDirection * -1);
                Task<Ply> calculation = CalculatePly(newPly,
                    nextTeam,
                    currentPlyDepth, minimaxDirection * -1);
                await calculation;

                parentPly.bestFuture = IsBest(parentPly.bestFuture, minimaxDirection, calculation.Result);
                newPly.originPly = parentPly;
                parentPly.futurePlies.Add(newPly);

                PieceMovementState.enPassantFlags = parentPly.enPassantFlags;
                ResetBoard(newPly);
            }
        }
        return parentPly.bestFuture;
    }

    Ply IsBest(Ply ply, int minimaxDirection, Ply potentialBest)
    {
        if (minimaxDirection == 1)
        {
            if (potentialBest.score > ply.score)
            {
                return potentialBest;
            }
            return ply;
        }
        else
        {
            if (potentialBest.score < ply.score)
            {
                return potentialBest;
            }
            return ply;
        }
    }

    Ply CreateSnapShot()
    {
        Ply ply = new Ply();
        ply.greens = new List<PieceEvaluation>();
        ply.golds = new List<PieceEvaluation>();

        foreach (Piece p in Board.instance.goldPieces)
        {
            if (p.gameObject.activeSelf)
            {
                ply.golds.Add(CreateEvaluationsPiece(p, ply));
            }
        }

        foreach (Piece p in Board.instance.greenPieces)
        {
            if (p.gameObject.activeSelf)
            {
                ply.greens.Add(CreateEvaluationsPiece(p, ply));
            }
        }
        return ply;
    }

    Ply CreateSnapShot(Ply parentPly)
    {
        Ply ply = new Ply();
        ply.greens = new List<PieceEvaluation>();
        ply.golds = new List<PieceEvaluation>();

        foreach (PieceEvaluation p in parentPly.golds)
        {
            if (p.piece.gameObject.activeSelf)
            {
                ply.golds.Add(CreateEvaluationsPiece(p.piece, ply));
            }
        }

        foreach (PieceEvaluation p in parentPly.greens)
        {
            if (p.piece.gameObject.activeSelf)
            {
                ply.greens.Add(CreateEvaluationsPiece(p.piece, ply));
            }
        }
        return ply;
    }

    PieceEvaluation CreateEvaluationsPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
        Board.instance.selectedPiece = eva.piece;
        eva.availableMoves = eva.piece.moviment.GetValidMoves();
        return eva;
    }

    void EvaluateBoard(Ply ply)
    {
        foreach (PieceEvaluation piece in ply.golds)
        {
            EvaluatePiece(piece, ply, 1);
        }
        foreach (PieceEvaluation piece in ply.greens)
        {
            EvaluatePiece(piece, ply, -1);
        }
    }

    void EvaluatePiece(PieceEvaluation eva, Ply ply, int scoreDirection)
    {
        int positionValue = eva.piece.moviment.positionValue[eva.piece.tile.pos];
        ply.score += (eva.piece.moviment.value + positionValue) * scoreDirection;
    }

    void ResetBoard(Ply ply)
    {
        foreach (AffectedPiece p in ply.changes)
        {
            p.Undo();
        }
    }

    void PrintBestPly(Ply finalPly)
    {
        Ply currentPly = finalPly;
        Debug.Log("Best movement: ");
        while (currentPly.originPly != null)
        {
            Debug.LogFormat("{0} - {1} -> {2}",
            currentPly.changes[0].piece.transform.parent.name,
            currentPly.changes[0].piece.name,
            currentPly.changes[0].to.pos);
            currentPly = currentPly.originPly;
        }
    }
}
