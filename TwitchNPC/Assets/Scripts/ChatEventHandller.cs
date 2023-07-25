using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ChatEventHandller : MonoBehaviour
{
    private struct MessageData
    {
        public string Chatter;
        public string Message;
    }

    private Animator _animator = null;
    private Queue<MessageData> _actionQueue = new Queue<MessageData>();
    private bool _isInAction = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        FindFirstObjectByType<TwitchConnect>().OnChatMessage.AddListener(HandleChatMessage);
    }

    private void Update()
    {
        if (_isInAction)
        {
            return;
        }

        if (_actionQueue.TryDequeue(out MessageData action))
        {
            PerformAction(action);
            _isInAction = true;
        }
    }

    private void PerformAction(MessageData action)
    {
        string chatter = action.Chatter;
        string message = action.Message;

        switch (message[0])
        {
            case '!':
                Debug.Log(message.Substring(1));
                _animator.SetTrigger(message.Substring(1));
                break;
            case '?':
                Debug.Log($"@{chatter}, {Reply(message.Substring(1))}");
                break;
        }
        
    }

    private string Reply(string message)
    {
        return "Hello, I am Motoko";
    }

    private void HandleChatMessage(string chatter, string message)
    {
        _actionQueue.Enqueue(new MessageData { Chatter = chatter, Message = message });
    }

    public void ActionEnded()
    {
        _isInAction = false;
    }
}
