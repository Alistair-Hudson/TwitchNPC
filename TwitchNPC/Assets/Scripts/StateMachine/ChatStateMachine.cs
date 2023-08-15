using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatStateMachine : StateMachine
{
    public struct MessageData
    {
        public string Chatter;
        public string Message;
    }

    public Animator Animator { get; private set; } = null;
    public Queue<MessageData> ActionQueue { get; private set; } = new Queue<MessageData>();

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FindFirstObjectByType<TwitchConnect>().OnChatMessage.AddListener(HandleChatMessage);

        SwitchState(new IdleState(this));
    }

    private void HandleChatMessage(string chatter, string message)
    {
        ActionQueue.Enqueue(new MessageData { Chatter = chatter, Message = message });
    }
}
