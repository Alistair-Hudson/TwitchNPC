using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftState : State
{
    private string _gift = "";

    public GiftState(StateMachine stateMachine, string gift) : base(stateMachine)
    {
        _gift = gift;
    }

    public override void Enter()
    {
        Debug.Log($"Thank  you for the {_gift}");
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
