using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ShowdownClient : MonoBehaviour
{
    [Header("States")]
    [SerializeField]
    ClientState LoginState;

    ClientState State;

    void Awake()
    {
        Assert.IsNotNull(LoginState, "LoginState is null");
    }

    void Start()
    {
        WebsocketConnection.Instance.OnMessageReceived += MessageReceived;
        ChangeState(LoginState);
    }

    public void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        ServerMessage m = MessageParser.Parse(e.Message);
        if(m != null)
            State.ReceiveMessage(this, m);
    }

    void ChangeState(ClientState newState)
    {
        if (State != null)
            State.LeaveState();

        State = newState;

        if (State != null)
            State.EnterState();
    }
}
