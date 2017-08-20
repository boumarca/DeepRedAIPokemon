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
        //Parse message
        //Send message to state
        State.ReceiveMessage(this, e.Message);
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
