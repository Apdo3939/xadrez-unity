using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameEndState : State
{
    public override async void Enter()
    {
        await Task.Delay(100);
        Debug.Log("Game Over");
    }

}
