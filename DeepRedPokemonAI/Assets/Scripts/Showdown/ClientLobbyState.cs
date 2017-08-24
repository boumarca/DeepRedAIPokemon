using System.Collections;
using System.Collections.Generic;
using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using com.MAB.Web;

namespace DeepRedAI.Showdown
{
    public class ClientLobbyState : ClientState
    {
        const string LeftBracket = "[";
        const string RightBracket = "]";
        const string Space = " ";

        [SerializeField]
        Dropdown _formats;
        [SerializeField]
        Text _userLabel;

        public override void EnterState(ShowdownClient context)
        {
            base.EnterState(context);
            PopulateFormats(context.FormatList);
            SetUserLabel(context.Username);
        }

        public override void ReceiveMessage(ServerMessage message)
        {
        }

        public void PlayLadder()
        {
            string formatId = CurrentFormatId();
            Debug.Log(formatId);
        }

        public void Logout()
        {
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Logout);
            WebsocketConnection.Instance.Send(command);
            _context.GoToLogin();
        }

        void PopulateFormats(List<string> formatsList)
        {
            _formats.AddOptions(formatsList);
        }

        string CurrentFormatId()
        {
            string formatText = _formats.options[_formats.value].text;
            StringBuilder s = new StringBuilder(formatText);
            s.Replace(LeftBracket, string.Empty);
            s.Replace(RightBracket, string.Empty);
            s.Replace(Space, string.Empty);
            return s.ToString().ToLower();
        }

        void SetUserLabel(string user)
        {
            _userLabel.text = user;
        }
    }
}
