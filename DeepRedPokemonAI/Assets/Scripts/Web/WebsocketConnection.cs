using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;

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

    WebSocket Socket;
    Queue<string> MessagesToConsume;

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

    void Awake ()
    {
        Instance = this;
        MessagesToConsume = new Queue<string>();
    }
	
    public void OpenSocket(string url)
    {
        Socket = new WebSocket(url);
        Socket.OnOpen += DebugOnOpen;
        Socket.OnClose += DebugOnClose;
        Socket.OnError += DebugOnError;
        Socket.OnMessage += DebugOnMessage;
        Socket.OnMessage += MessageReceived;
        Socket.Connect();
    }

    public void CloseSocket()
    {
        Socket.OnOpen -= DebugOnOpen;
        Socket.OnClose -= DebugOnClose;
        Socket.OnError -= DebugOnError;
        Socket.OnMessage -= DebugOnMessage;
        Socket.OnMessage -= MessageReceived;
        Socket.Close();
        Socket = null;
    }

    public void Send(string message)
    {
        if (Socket != null)
        {
            if (LogLevel <= LogLevel.Debug)
                Debug.Log("Send Message " + message);
            Socket.Send(message);
        }
    }

    void EnqueueMessage(string message)
    {
        lock(Instance)     
        {
            MessagesToConsume.Enqueue(message);
        }
    }

    string DequeueMessage()
    {
        lock(Instance)
        {
            if(MessagesToConsume.Count > 0)
                return MessagesToConsume.Dequeue();
            return null;
        }
    }

    void Update()
    {
        if (MessagesToConsume.Count > 0)
        {
            string message = DequeueMessage();
            if (!string.IsNullOrEmpty(message))
                OnMessageReceivedInternal(this, new MessageReceivedEventArgs(message));
        }
    }

    void Destroy()
    {
        CloseSocket();
    }

    void MessageReceived(object sender, MessageEventArgs e)
    {
        if(e != null)
            EnqueueMessage(e.Data);
    }

    void DebugOnOpen(object sender, EventArgs e)
    {
        if(LogLevel <= LogLevel.Debug)
            Debug.Log("OnOpen " + e.ToString());
    }

    void DebugOnClose(object sender, CloseEventArgs e)
    {
        if (LogLevel <= LogLevel.Debug)
            Debug.Log("OnClose " + e.Reason);
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
