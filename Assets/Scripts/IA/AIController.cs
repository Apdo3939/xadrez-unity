using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;
    public Ply currentState;
    public HighlightsClick AIhighlight;

    void Awake()
    {
        instance = this;
    }

    [ContextMenu("Calculate Plays")]
    public async void CalculatePlay()
    {
        currentState = CreateSnapShot();
        currentState.name = "start";
        EvaluateBoard(currentState);

        Ply currentPly = currentState;
        currentPly.originPly = null;
        currentPly.futurePlies = new List<Ply>();
        Debug.Log("start");
        foreach (PieceEvaluation eva in currentPly.golds)
        {
            Debug.Log("Analisando eva de " + eva.piece);
            foreach (Tile t in eva.availableMoves)
            {
                Debug.Log("Analisando t: " + t.pos);
                Board.instance.selectedPiece = eva.piece;
                Board.instance.selectedHighlight = AIhighlight;
                AIhighlight.tile = t;
                AIhighlight.transform.position = new Vector3(t.pos.x, t.pos.y, 0);
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true);
                await tcs.Task;
                Ply newPly = CreateSnapShot();
                newPly.name = string.Format("{0}, {1} to {2}", currentPly.name, eva.piece, t.pos);
                newPly.changes = PieceMovementState.changes;
                Debug.Log(newPly.name);
                EvaluateBoard(newPly);
                newPly.moveType = t.moveType;
                currentPly.futurePlies.Add(newPly);
                ResetBoard(newPly);
            }
            Debug.Log(currentPly.futurePlies.Count);
            currentPly.futurePlies.Sort((x, y) => x.score.CompareTo(y.score));
            Debug.Log("Would choose: " + currentPly.futurePlies[0].name);
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

    PieceEvaluation CreateEvaluationsPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
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

        Debug.Log("Board score: " + ply.score);
    }

    void EvaluatePiece(PieceEvaluation eva, Ply ply, int scoreDirection)
    {
        Board.instance.selectedPiece = eva.piece;
        eva.availableMoves = eva.piece.moviment.GetValidMoves();


        eva.score = eva.piece.moviment.value;
        ply.score += eva.score * scoreDirection;
    }

    void ResetBoard(Ply ply)
    {
        foreach (AffectedPiece p in ply.changes)
        {
            p.piece.tile.content = null;
            p.piece.tile = p.from;
            p.from.content = p.piece;
            p.piece.transform.position = new Vector3(p.from.pos.x, p.from.pos.y, 0);
            p.piece.gameObject.SetActive(true);
        }
    }
}
