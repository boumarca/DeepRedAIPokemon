using com.MAB.Web;
using DeepRedAI.Parser;
using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DeepRedAI.Showdown
{
    public class ClientLoginState : ClientState
    {
        const string LoginURL = "http://play.pokemonshowdown.com/action.php";
        const string PostRequestData = "act=login&name={0}&pass={1}&challstr={2}";

        [Header("UI Elements")]
        [SerializeField]
        InputField _usernameField = default;
        [SerializeField]
        InputField _passwordField = default;
        [SerializeField]
        Text _errorText = default;

        [Header("Data")]
        [SerializeField]
        ServerArgs _serverArgs = default;

        string _challstr;
        
        void Start()
        {
            if (!string.IsNullOrEmpty(_serverArgs.ServerUrl))
            {
                WebsocketConnection.Instance.OpenSocket(_serverArgs.ServerUrl);
            }
        }

        public override void ReceiveMessage(ServerMessage message)
        {
            ServerMessageData[] payload = message.Payload;
            for (int i = 0; i < payload.Length; i++)
            {
				ServerMessageData serverMessageData = payload[i];
                if (serverMessageData.Type == MessageDataType.ChallStr)
                    _challstr = string.Join("|", serverMessageData.Data);
                else if (serverMessageData.Type == MessageDataType.UpdateUser)
                    UpdateUser(payload[i].Data);
                else if (serverMessageData.Type == MessageDataType.Formats)
                    _context.PopulateFormatList(serverMessageData.Data);
                else if (serverMessageData.Type == MessageDataType.NameTaken)
                    LogErrorOnScreen(serverMessageData.Data);
            }
        }

        public void Login()
        {
            _errorText.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(_usernameField.text) && !string.IsNullOrEmpty(_passwordField.text))
            {
                StartCoroutine(LoginRoutine());
            }
        }

        public void LoginAsGuest()
        {
            _errorText.gameObject.SetActive(false);
            _context.GoToLobby();
        }

        IEnumerator LoginRoutine()
        {
            WWWForm form = new WWWForm();
            form.AddField("act", "login");
            form.AddField("name", _usernameField.text);
            form.AddField("pass", _passwordField.text);
            form.AddField("challstr", _challstr);

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
                string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Trn, _usernameField.text, "0", node["assertion"]);
                WebsocketConnection.Instance.Send(command);
            }
        }
        
        void UpdateUser(string[] data)
        {
            _errorText.gameObject.SetActive(false);
            _context.Username = data[0];
            if (data[1] == "1")
            {
                _context.GoToLobby();
            }
        }

        void LogErrorOnScreen(string[] data)
        {
            _errorText.text = data[1];
            _errorText.gameObject.SetActive(true);
        }
    }
}
