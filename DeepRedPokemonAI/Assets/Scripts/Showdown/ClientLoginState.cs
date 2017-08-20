using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientLoginState : ClientState
{
    [Header("UI Elements")]
    [SerializeField]
    InputField UsernameField;
    [SerializeField]
    InputField PasswordField;

    [Header("Data")]
    [SerializeField]
    string ServerUrl;

    string Username;
    string Password;

    public void Login()
    {
        if (!string.IsNullOrEmpty(UsernameField.text) && !string.IsNullOrEmpty(PasswordField.text) && !string.IsNullOrEmpty(ServerUrl))
        {
            Username = UsernameField.text;
            Password = PasswordField.text;
            WebsocketConnection.Instance.OpenSocket(ServerUrl);
        }
    }

    public override void ReceiveMessage(ShowdownClient context, string message)
    {
        //Do stuff with message
        Debug.Log("MESSAGE: " + message);
    }
}
