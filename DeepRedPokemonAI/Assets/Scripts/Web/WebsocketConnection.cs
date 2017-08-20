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

public class WebsocketConnection : MonoBehaviour
{
    public static WebsocketConnection Instance { get; private set; }   

    WebSocket Socket;
    Queue<string> MessagesToConsume;

    public LogLevel LogLevel = LogLevel.Debug;

    public event EventHandler OnOpen 
    {
        add
        {
            if (Socket != null)
                Socket.OnOpen += value;
        }
        remove
        {
            if (Socket != null)
                Socket.OnOpen -= value;
        }
    }

    public event EventHandler<CloseEventArgs> OnClose
    {
        add
        {
            if (Socket != null)
                Socket.OnClose += value;
        }
        remove
        {
            if (Socket != null)
                Socket.OnClose -= value;
        }
    }

    public event EventHandler<ErrorEventArgs> OnError
    {
        add
        {
            if (Socket != null)
                Socket.OnError += value;
        }
        remove
        {
            if (Socket != null)
                Socket.OnError -= value;
        }
    }

    public event EventHandler<MessageEventArgs> OnMessage
    {
        add
        {
            if (Socket != null)
                Socket.OnMessage += value;
        }
        remove
        {
            if (Socket != null)
                Socket.OnMessage -= value;
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
        OnOpen += DebugOnOpen;
        OnClose += DebugOnClose;
        OnError += DebugOnError;
        OnMessage += DebugOnMessage;
        OnMessage += OnMessageReceived;         
    }

    public void Connect()
    {
        if (Socket != null)
            Socket.Connect();
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

    public void EnqueueMessage(string message)
    {
        lock(Instance)     
        {
            MessagesToConsume.Enqueue(message);
        }
    }

    public string DequeueMessage()
    {
        lock(Instance)
        {
            if(HasMessage())
                return MessagesToConsume.Dequeue();
            return null;
        }
    }

    public bool HasMessage()
    {
        return MessagesToConsume.Count > 0;
    }

    void Destroy()
    {
        OnOpen -= DebugOnOpen;
        OnClose -= DebugOnClose;
        OnError -= DebugOnError;
        OnMessage -= DebugOnMessage;
        OnMessage -= OnMessageReceived;
        Socket.Close();
    }

    void OnMessageReceived(object sender, MessageEventArgs e)
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
