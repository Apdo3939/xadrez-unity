using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    public override async void Enter()
    {
        Debug.Log("Piece Moved...");
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        MovePiece(tcs, false);
        await tcs.Task;
        machine.ChangeTo<TurnEndState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public static void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        changes = new List<AffectedPiece>();
        MoveType moveType = Board.instance.selectedHighlight.tile.moveType;
        ClearEnPassants();

        switch (moveType)
        {
            case MoveType.Normal:
                NormalMove(tcs, skipMovements);
                break;
            case MoveType.Castling:
                Castling(tcs, skipMovements);
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove(tcs, skipMovements);
                break;
            case MoveType.EnPassant:
                EnPassant(tcs, skipMovements);
                break;
            case MoveType.Promotion:
                Promotion(tcs, skipMovements);
                break;
        }
    }

    static void NormalMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece piece = Board.instance.selectedPiece;

        AffectedPiece pieceMoving = new AffectedPiece();
        pieceMoving.piece = piece;
        pieceMoving.from = piece.tile;
        pieceMoving.to = Board.instance.selectedHighlight.tile;
        pieceMoving.wasMoved = piece.wasMoved;
        changes.Add(pieceMoving);

        piece.tile.content = null;
        piece.tile = Board.instance.selectedHighlight.tile;

        if (piece.tile.content != null)
        {
            Piece deadPiece = piece.tile.content;

            AffectedPiece pieceKilled = new AffectedPiece();
            pieceKilled.piece = deadPiece;
            pieceKilled.from = pieceKilled.to = piece.tile;
            changes.Add(pieceKilled);
            //Debug.Log("Piece {0}... removida", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }

        piece.tile.content = piece;
        piece.wasMoved = true;

        if (skipMovements)
        {
            piece.wasMoved = true;
            //piece.transform.position = Board.instance.selectedHighlight.transform.position;
            tcs.SetResult(true);
        }
        else
        {
            piece.wasMoved = true; //add for IA
            float timing = Vector3.Distance(piece.transform.position, Board.instance.selectedHighlight.transform.position) * 0.5f;
            LeanTween.move(piece.gameObject, Board.instance.selectedHighlight.transform.position, timing).setOnComplete(() =>
            {
                tcs.SetResult(true);
            });
        }
    }

    static void Castling(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece king = Board.instance.selectedPiece;
        king.tile.content = null;
        Piece rock = Board.instance.selectedHighlight.tile.content;
        rock.tile.content = null;

        Vector2Int direction = rock.tile.pos - king.tile.pos;

        if (direction.x > 0)
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x + 2, king.tile.pos.y)];
            rock.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x - 1, king.tile.pos.y)];
        }
        else
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x - 2, king.tile.pos.y)];
            rock.tile = Board.instance.tiles[new Vector2Int(king.tile.pos.x + 1, king.tile.pos.y)];
        }

        king.tile.content = king;
        rock.tile.content = rock;
        king.wasMoved = true;
        rock.wasMoved = true;

        float timingKing = Vector3.Distance(king.transform.position, Board.instance.selectedHighlight.transform.position) * 0.5f;
        LeanTween.move(king.gameObject, new Vector3(king.tile.pos.x, king.tile.pos.y, 0), timingKing).setOnComplete(() =>
       {
           tcs.SetResult(true);
       });

        LeanTween.move(rock.gameObject, new Vector3(rock.tile.pos.x, rock.tile.pos.y, 0), timingKing - 0.1f);
    }

    static void ClearEnPassants()
    {
        ClearEnPassants(5);
        ClearEnPassants(2);
    }

    static void ClearEnPassants(int height)
    {
        Vector2Int positions = new Vector2Int(0, height);
        for (int i = 0; i < 7; i++)
        {
            positions.x = positions.x + 1;
            Board.instance.tiles[positions].moveType = MoveType.Normal;

        }
    }

    static void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.tile.pos.y > Board.instance.selectedHighlight.tile.pos.y ? new Vector2Int(0, -1) :
        new Vector2Int(0, 1);
        Board.instance.tiles[pawn.tile.pos + direction].moveType = MoveType.EnPassant;
        NormalMove(tcs, skipMovements);
    }
    static void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.tile.pos.y > Board.instance.selectedHighlight.tile.pos.y ? new Vector2Int(0, 1) :
        new Vector2Int(0, -1);
        Tile enemy = Board.instance.tiles[Board.instance.selectedHighlight.tile.pos + direction];
        enemy.content.gameObject.SetActive(false);
        enemy.content = null;
        NormalMove(tcs, skipMovements);
    }

    static async void Promotion(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        TaskCompletionSource<bool> movementTcs = new TaskCompletionSource<bool>();
        NormalMove(movementTcs, skipMovements);
        await movementTcs.Task;
        Debug.Log("Promotion");

        StateMachineController.instance.taskHold = new TaskCompletionSource<object>();
        StateMachineController.instance.promotionPanel.SetActive(true);

        await StateMachineController.instance.taskHold.Task;

        string result = StateMachineController.instance.taskHold.Task.Result as string;

        if (result == "Knight")
        {
            Board.instance.selectedPiece.moviment = new KnightMovement();
        }
        else
        {
            Board.instance.selectedPiece.moviment = new QueenMovement();
        }
        StateMachineController.instance.promotionPanel.SetActive(false);
        tcs.SetResult(true);

    }
}
