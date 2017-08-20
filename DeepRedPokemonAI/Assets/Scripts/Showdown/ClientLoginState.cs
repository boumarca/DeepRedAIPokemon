using com.MAB.Web;
using DeepRedAI.Parser;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DeepRedAI.Showdown
{
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

        private void Awake()
        {
            Assert.IsNotNull(UsernameField, "Username field is null");
            Assert.IsNotNull(PasswordField, "Password field is null");
            Assert.IsFalse(string.IsNullOrEmpty(ServerUrl), "Url is null or empty.");
        }

        public void Login()
        {
            if (!string.IsNullOrEmpty(UsernameField.text) && !string.IsNullOrEmpty(PasswordField.text) && !string.IsNullOrEmpty(ServerUrl))
            {
                Username = UsernameField.text;
                Password = PasswordField.text;
                WebsocketConnection.Instance.OpenSocket(ServerUrl);
            }
        }

        public override void ReceiveMessage(ShowdownClient context, ServerMessage message)
        {
        //Do stuff with message
        Debug.Log("MESSAGE: " + message);
    }
}
