using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowdownClient : MonoBehaviour
{
    [SerializeField]
    private string ServerUrl;

	// Use this for initialization
	void Start ()
    {
        Connect();
	}

    public void Connect()
    {
        if (!string.IsNullOrEmpty(ServerUrl))
        {
            WebsocketConnection.Instance.OpenSocket(ServerUrl);
            WebsocketConnection.Instance.Connect();
        }
    }
}
