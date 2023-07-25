using OpenAi.Unity.V1;
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
            _isInAction = true;
            PerformAction(action);
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
                StartCoroutine(Reply(action.Chatter, message.Substring(1)));
                break;
        }
        
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
        _isInAction = false;
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
