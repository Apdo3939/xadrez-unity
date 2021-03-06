using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    public static AvailableMoves enPassantFlags;
    public override async void Enter()
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        MovePiece(tcs, false, Board.instance.selectedMove.moveType);
        await tcs.Task;
        machine.ChangeTo<TurnEndState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public static void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovements, MoveType moveType)
    {
        changes = new List<AffectedPiece>();
        enPassantFlags = new AvailableMoves();
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
        AffectedPiece pieceMoving = piece.CreatedAffected();
        pieceMoving.piece = piece;
        pieceMoving.from = piece.tile;
        pieceMoving.to = Board.instance.tiles[Board.instance.selectedMove.pos];
        changes.Insert(0, pieceMoving);

        piece.tile.content = null;
        piece.tile = pieceMoving.to;

        if (piece.tile.content != null)
        {
            Piece deadPiece = piece.tile.content;
            AffectedEnemy pieceKilled = new AffectedEnemy();
            pieceKilled.piece = deadPiece;
            pieceKilled.to = pieceKilled.from = piece.tile;
            changes.Add(pieceKilled);
            deadPiece.gameObject.SetActive(false);
            pieceKilled.index = deadPiece.team.IndexOf(deadPiece);
            deadPiece.team.RemoveAt(pieceKilled.index);
        }

        piece.tile.content = piece;
        piece.wasMoved = true;

        if (skipMovements)
        {
            piece.wasMoved = true;
            tcs.SetResult(true);
        }
        else
        {
            Vector3 vPos = new Vector3(Board.instance.selectedMove.pos.x, Board.instance.selectedMove.pos.y, 0);
            float timing = Vector3.Distance(piece.transform.position, vPos) * 0.5f;
            LeanTween.move(piece.gameObject, vPos, timing).setOnComplete(() =>
            {
                tcs.SetResult(true);
            });
        }
    }
    static void Castling(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece king = Board.instance.selectedPiece;
        AffectedPieceKingRock affectedKing = new AffectedPieceKingRock();
        affectedKing.from = king.tile;
        king.tile.content = null;
        affectedKing.piece = king;

        Piece rock = Board.instance.tiles[Board.instance.selectedMove.pos].content;
        AffectedPieceKingRock affectedRock = new AffectedPieceKingRock();
        affectedRock.from = rock.tile;
        rock.tile.content = null;
        affectedRock.piece = rock;

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
        affectedKing.to = king.tile;
        changes.Add(affectedKing);
        rock.tile.content = rock;
        affectedRock.to = rock.tile;
        changes.Add(affectedRock);
        king.wasMoved = true;
        rock.wasMoved = true;

        if (skipMovements)
        {
            tcs.SetResult(true);
        }
        else
        {
            LeanTween.move(king.gameObject, new Vector3(king.tile.pos.x, king.tile.pos.y, 0), 1.5f).setOnComplete(() => { tcs.SetResult(true); });
            LeanTween.move(rock.gameObject, new Vector3(rock.tile.pos.x, rock.tile.pos.y, 0), 1.4f);
        }
    }
    static void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, 1) :
            new Vector2Int(0, -1);
        enPassantFlags = new AvailableMoves(pawn.tile.pos + direction, MoveType.EnPassant);
        NormalMove(tcs, skipMovements);
    }
    static void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, -1) :
            new Vector2Int(0, 1);
        Tile enemy = Board.instance.tiles[Board.instance.selectedMove.pos + direction];
        AffectedEnemy affectedEnemy = new AffectedEnemy();
        affectedEnemy.from = affectedEnemy.to = enemy;
        affectedEnemy.piece = enemy.content;
        affectedEnemy.index = affectedEnemy.piece.team.IndexOf(affectedEnemy.piece);
        affectedEnemy.piece.team.RemoveAt(affectedEnemy.index);
        changes.Add(affectedEnemy);
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
        Pawn pawn = Board.instance.selectedPiece as Pawn;

        if (!skipMovements)
        {
            StateMachineController.instance.taskHold = new TaskCompletionSource<object>();
            StateMachineController.instance.promotionPanel.SetActive(true);
            await StateMachineController.instance.taskHold.Task;
            string result = StateMachineController.instance.taskHold.Task.Result as string;

            if (result == "Knight")
            {
                Board.instance.selectedPiece.moviment = pawn.knightMovement;
            }
            else
            {
                Board.instance.selectedPiece.moviment = pawn.queenMovement;
            }
            StateMachineController.instance.promotionPanel.SetActive(false);
        }
        else
        {
            AffectedPiecePawn aff = new AffectedPiecePawn();
            aff.wasMoved = true;
            aff.resetMovement = true;
            aff.from = changes[0].from;
            aff.to = changes[0].to;
            aff.piece = pawn;

            changes[0] = aff;
            pawn.moviment = pawn.queenMovement;

        }
        tcs.SetResult(true);
    }
}
