using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;
    public Ply currentState;

    void Awake()
    {
        instance = this;
    }

    [ContextMenu("Create Evaluations")]
    public void CreateEvaluations()
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

        currentState = ply;
    }

    PieceEvaluation CreateEvaluationsPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
        return eva;
    }

    [ContextMenu("Evaluate")]
    public void EvaluateBoard()
    {
        Ply ply = currentState;
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
        List<Tile> tiles = eva.piece.moviment.GetValidMoves();
        eva.availableMoves = tiles.Count;

        eva.score = eva.piece.moviment.value;
        ply.score += eva.score * scoreDirection;
    }
}
