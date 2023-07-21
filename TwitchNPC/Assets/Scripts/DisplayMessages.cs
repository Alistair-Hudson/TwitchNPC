using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMessages : MonoBehaviour
{
    private TMP_Text _textDisplay = null;

    private void Awake()
    {
        _textDisplay = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        TwitchConnect twitchConnect = FindFirstObjectByType<TwitchConnect>();

        twitchConnect.OnChatMessage.AddListener(DisplayeMesage);
    }

    private void DisplayeMesage(string chat, string message)
    {
        _textDisplay.text = $"{message} from {chat}";
    }
}
