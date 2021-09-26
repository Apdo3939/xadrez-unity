using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn End");
        bool gameFinished = CheckConditions();
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

    bool CheckConditions()
    {
        if (CheckKing() || CheckTeams())
        {
            return true;
        }
        return false;
    }

    bool CheckKing()
    {
        King king = Board.instance.goldHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            Debug.Log("Green Win!!!!");
            return true;
        }
        king = Board.instance.greenHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            Debug.Log("Gold Win!!!!");
            return true;
        }
        return false;
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
