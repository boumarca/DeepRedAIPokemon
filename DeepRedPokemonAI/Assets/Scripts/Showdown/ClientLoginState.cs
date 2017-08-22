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
        InputField _usernameField;
        [SerializeField]
        InputField _passwordField;

        [Header("Data")]
        [SerializeField]
        string _serverUrl;

        string[] _challstr;
        string _username;

        void Awake()
        {
            Assert.IsNotNull(_usernameField, "Username field is null");
            Assert.IsNotNull(_passwordField, "Password field is null");
            Assert.IsFalse(string.IsNullOrEmpty(_serverUrl), "Url is null or empty.");
        }

        void Start()
        {
            if (!string.IsNullOrEmpty(_serverUrl))
            {
                WebsocketConnection.Instance.OpenSocket(_serverUrl);
            }
        }

        public override void ReceiveMessage(ShowdownClient context, ServerMessage message)
        {
            ServerMessageData[] payload = message.Payload;
            for (int i = 0; i < payload.Length; i++)
            {
                if (payload[i].Type == MessageDataType.DataType.ChallStr)
                    _challstr = payload[i].Data;
                else if (payload[i].Type == MessageDataType.DataType.UpdateUser)
                    UpdateUser(context, payload[i].Data);
                else if (payload[i].Type == MessageDataType.DataType.Formats)
                    context.PopulateFormatList(payload[i].Data);
            }
        }

        public void Login()
        {
            if (!string.IsNullOrEmpty(_usernameField.text) && !string.IsNullOrEmpty(_passwordField.text))
            {
                StartCoroutine(LoginRoutine());
            }
        }

        IEnumerator LoginRoutine()
        {
            WWWForm form = new WWWForm();
            form.AddField("act", "login");
            form.AddField("name", _usernameField.text);
            form.AddField("pass", _passwordField.text);
            form.AddField("challstr", string.Join("|", _challstr));

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
                _username = node["curuser"]["username"];
                string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.DataType.Trn, _username, "0", node["assertion"]);
                WebsocketConnection.Instance.Send(command);
            }
        }
        
        void UpdateUser(ShowdownClient context, string[] data)
        {
            if(data.Length == 3 && data[0] == _username && data[1] == "1")
            {
                context.GoToLobby();
            }
        }
    }
}
