using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        (stateMachine as ChatStateMachine).Animator.CrossFadeInFixedTime("Idle", 0.1f);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        if ((stateMachine as ChatStateMachine).ActionQueue.TryDequeue(out var action))
        {
            SelectState(action);
        }
    }

    private void SelectState(ChatStateMachine.MessageData action)
    {
        string chatter = action.Chatter;
        string message = action.Message;

        switch (message[0])
        {
            case '!':
                Debug.Log(message.Substring(1));
                stateMachine.SwitchState(new ActionState(stateMachine, message.Substring(1)));
                break;
            case '?':
                stateMachine.SwitchState(new ChatState(stateMachine, chatter, message.Substring(1)));
                break;
            case '>':
                stateMachine.SwitchState(new GiftState(stateMachine, message.Substring(1)));
                break;
        }

    }
}
