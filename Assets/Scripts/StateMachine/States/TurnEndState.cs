using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn End");
        bool gameFinished = CheckTeams();
        await Task.Delay(100);
        if (gameFinished)
        {
            machine.ChangeTo<GameEndState>();
        }
        else
        {
            machine.ChangeTo<TurnBeginState>();
        }
    }


    bool CheckTeams()
    {
        Piece goldPiece = Board.instance.goldPieces.Find((x) => x.gameObject.activeSelf == true);
        if (goldPiece == null)
        {
            Debug.Log("Green Wins!!!");
            return true;
        }

        Piece greenPiece = Board.instance.greenPieces.Find((x) => x.gameObject.activeSelf == true);
        if (greenPiece == null)
        {
            Debug.Log("Gold Wins!!!");
            return true;
        }

        return false;

    }
}
