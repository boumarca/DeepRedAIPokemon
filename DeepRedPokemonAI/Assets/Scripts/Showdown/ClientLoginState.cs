using com.MAB.Web;
using DeepRedAI.Parser;
using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DeepRedAI.Showdown
{
    public class ClientLoginState : ClientState
    {
        const string LoginURL = "http://play.pokemonshowdown.com/action.php";
        const string PostRequestData = "act=login&name={0}&pass={1}&challstr={2}";

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
            ServerMessageData[] payload = message.Payload;
            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i].Type == MessageDataType.DataType.ChallStr)
                    StartCoroutine(Login(context, payload[i].Data));
            }
        }

        IEnumerator Login(ShowdownClient context, string[] data)
        {
            WWWForm form = new WWWForm();
            form.AddField("act", "login");
            form.AddField("name", Username);
            form.AddField("pass", Password);
            form.AddField("challstr", string.Join("|", data));

            WWW www = new WWW(LoginURL, form);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
            }
            else if (!string.IsNullOrEmpty(www.text) && www.text.Length >= 1)
            {
                string json = www.text.Remove(0, 1);
                JSONNode node = JSON.Parse(json);
                string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.DataType.Trn, Username, "0", node["assertion"]);
                WebsocketConnection.Instance.Send(command);
            }
        }
        
    }
}
