using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LoadState : State
{
    public override async void Enter()
    {
        Debug.Log("Entrou no estado");
        await Board.instance.Load();
        await LoadAllPiecesAsync();
        machine.currentlyPlaying = machine.player2;
        machine.ChangeTo<TurnBeginState>();
    }

    async Task LoadAllPiecesAsync()
    {
        LoadTeamPieces(Board.instance.goldPieces);
        LoadTeamPieces(Board.instance.greenPieces);
        await Task.Delay(100);
    }

    void LoadTeamPieces(List<Piece> pieces)
    {
        foreach (Piece p in pieces)
        {
            Board.instance.AddPiece(p.transform.parent.name, p);
        }
    }
}
