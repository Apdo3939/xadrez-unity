using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayingState : State
{
    public async override void Enter()
    {
        Task<Ply> task = AIController.instance.CalculatePlays();
        await task;
        Ply bestResult = task.Result;
        MakeBestPlay(bestResult);
    }

    async void MakeBestPlay(Ply bestResult)
    {
        Ply currentPly = bestResult;
        for (int i = 1; i < AIController.instance.objectivePlyDepth; i++)
        {
            currentPly = currentPly.originPly;
        }
        Board.instance.selectedPiece = currentPly.changes[0].piece;
        Board.instance.selectedMove = GetMoveType(currentPly);
        await Task.Delay(100);
        machine.ChangeTo<PieceMovementState>();
    }

    AvailableMoves GetMoveType(Ply ply)
    {
        List<PieceEvaluation> team;
        if (machine.currentlyPlaying == machine.player1)
        {
            team = ply.golds;
        }
        else
        {
            team = ply.greens;
        }
        List<AvailableMoves> moves = Board.instance.selectedPiece.moviment.GetValidMoves();
        foreach (AvailableMoves m in moves)
        {
            if (m.pos == ply.changes[0].to.pos)
            {
                return m;
            }
        }
        return new AvailableMoves();
    }
}
