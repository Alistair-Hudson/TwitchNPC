using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.IO;

public class TwitchConnect : MonoBehaviour
{
    [SerializeField]
    private string _user = "";
    [SerializeField]
    [Tooltip("get from https://twitchapps.com/tmi")]
    private string _oAuth = "";
    [SerializeField]
    private string _channel = "";

    private TcpClient _twitch = null;
    private StreamReader _reader = null;
    private StreamWriter _writer = null;

    private float _pingCounter = 0;

    private const string URL = "irc.chat.twitch.tv";
    private const int PORT = 6667;

    public UnityEvent<string, string> OnChatMessage;

    private void Awake()
    {
        ConnectToTwitch();
    }

    private void Update()
    {
        _pingCounter += Time.deltaTime;
        if (_pingCounter > 60)
        {
            _writer.WriteLine("Ping " + URL);
            _writer.Flush();
            _pingCounter = 0;
        }

        if (!_twitch.Connected)
        {
            ConnectToTwitch();
        }

        if (_twitch.Available > 0)
        {
            string message = _reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                // :user!user@user.tmi.twitch.tv PRIVMSG #user :message
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string msg = message.Substring(splitPoint + 1);

                Debug.Log(chatter);
                Debug.Log(msg);
                OnChatMessage?.Invoke(chatter, msg);
            }
        }
    }
    private void ConnectToTwitch()
    {
        _twitch = new TcpClient(URL, PORT);
        _reader = new StreamReader(_twitch.GetStream());
        _writer = new StreamWriter(_twitch.GetStream());
        
        _writer.WriteLine("PASS " + _oAuth);
        _writer.WriteLine("Nick " + _user.ToLower());
        _writer.WriteLine("Join #" + _channel.ToLower());
        _writer.Flush();
    }
}
