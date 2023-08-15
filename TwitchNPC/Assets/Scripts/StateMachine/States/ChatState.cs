using OpenAi.Unity.V1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatState : State
{
    private string _chatter = "";
    private string _message = "";

    public ChatState(StateMachine stateMachine, string chatter, string message) : base(stateMachine)
    {
        _chatter = chatter;
        _message = message;
    }

    public override void Enter()
    {
        stateMachine.StartCoroutine(Reply(_chatter, _message));
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }

    private IEnumerator Reply(string chatter, string message)
    {
        string reply = "Hello, I am Motoko.";
        if (string.IsNullOrEmpty(message))
        {
            Debug.LogError("Example requires input in input field");
            yield break;
        }

        yield return OpenAiChatCompleterV1.Instance.Complete(
            message,
            s => reply = s,
            e => reply = $"ERROR: StatusCode: {e.responseCode} - {e.error}"
        );

        Debug.Log($"@{chatter}, {reply}");
        stateMachine.SwitchState(new IdleState(stateMachine));
    }
}
