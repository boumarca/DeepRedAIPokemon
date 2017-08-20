using System;
using UnityEngine;

public class ShowdownClient : MonoBehaviour
{
    [Header("States")]
    [SerializeField]
    ClientState LoginState;


    ClientState State;

    void Start()
    {
        WebsocketConnection.Instance.OnMessageReceived += MessageReceived;
        ChangeState(LoginState);
    }

    public void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        ServerMessage m = MessageParser.Parse(e.Message);
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
