using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClientState : MonoBehaviour
{
    public virtual void EnterState()
    {
        gameObject.SetActive(true);
    }

    public virtual void LeaveState()
    {
        gameObject.SetActive(false);
    }

    public abstract void ReceiveMessage(ShowdownClient context, string message);
}
