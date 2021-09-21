using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn begin");
        if (machine.currentlyPlaying == machine.player1)
        {
            machine.currentlyPlaying = machine.player2;
        }
        else
        {
            machine.currentlyPlaying = machine.player1;
        }

        Debug.Log("Turn begin " + machine.currentlyPlaying + " now playing");
        await Task.Delay(100);
        machine.ChangeTo<TurnEndState>();
    }
}
