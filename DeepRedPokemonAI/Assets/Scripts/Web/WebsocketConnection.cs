using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

namespace com.MAB.Web
{
    public enum LogLevel
    {
        Debug,
        Error,
        None
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }
    }

    public class WebsocketConnection : MonoBehaviour
    {
        public static WebsocketConnection Instance { get; private set; }

        WebSocket _socket;
        Queue<string> _messagesToConsume;

        public LogLevel LogLevel = LogLevel.Debug;

        private event EventHandler<MessageReceivedEventArgs> OnMessageReceivedInternal;
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived
        {
            add
            {
                OnMessageReceivedInternal += value;
            }
            remove
            {
                OnMessageReceivedInternal -= value;
            }
        }

        void Awake()
        {
            Instance = this;
            _messagesToConsume = new Queue<string>();
        }

        public void OpenSocket(string url)
        {
            _socket = new WebSocket(url);
            _socket.OnOpen += DebugOnOpen;
            _socket.OnClose += DebugOnClose;
            _socket.OnError += DebugOnError;
            _socket.OnMessage += DebugOnMessage;
            _socket.OnMessage += MessageReceived;
            _socket.Connect();
        }

        public void CloseSocket()
        {
            if (_socket == null)
                return;

            _socket.OnOpen -= DebugOnOpen;
            _socket.OnClose -= DebugOnClose;
            _socket.OnError -= DebugOnError;
            _socket.OnMessage -= DebugOnMessage;
            _socket.OnMessage -= MessageReceived;
            _socket.Close();
            _socket = null;			
        }

        public void Send(string message)
        {
            if (_socket != null)
            {
                if (LogLevel <= LogLevel.Debug)
                    Debug.Log("Send Message " + message);
                _socket.Send(message);
            }
        }

        void EnqueueMessage(string message)
        {
            lock (Instance)
            {
                _messagesToConsume.Enqueue(message);
            }
        }

        string DequeueMessage()
        {
            lock (Instance)
            {
                if (_messagesToConsume.Count > 0)
                    return _messagesToConsume.Dequeue();
                return null;
            }
        }

        void Update()
        {
            if (_messagesToConsume.Count > 0)
            {
                string message = DequeueMessage();
                if (!string.IsNullOrEmpty(message))
                    OnMessageReceivedInternal(this, new MessageReceivedEventArgs(message));
            }
        }

        void OnDestroy()
        {
            CloseSocket();
			Instance = null;
		}

        void MessageReceived(object sender, MessageEventArgs e)
        {
            if (e != null)
                EnqueueMessage(e.Data);
        }

        void DebugOnOpen(object sender, EventArgs e)
        {
            if (LogLevel <= LogLevel.Debug)
                Debug.Log("OnOpen " + e.ToString());
        }

        void DebugOnClose(object sender, CloseEventArgs e)
        {
            if (LogLevel <= LogLevel.Debug)
                Debug.Log("OnClose " + e.Reason + " code " + e.Code);
        }

        void DebugOnError(object sender, ErrorEventArgs e)
        {
            if (LogLevel <= LogLevel.Error)
                Debug.LogError("OnError " + e.Message);
        }

        void DebugOnMessage(object sender, MessageEventArgs e)
        {
            if (LogLevel <= LogLevel.Debug)
                Debug.Log("OnMessage " + e.Data);
        }
    }
}
