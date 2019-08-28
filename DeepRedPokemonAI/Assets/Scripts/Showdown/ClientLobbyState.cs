using System.Collections.Generic;
using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using com.MAB.Web;
using SimpleJSON;

namespace DeepRedAI.Showdown
{
    public class ClientLobbyState : ClientState
    {
        const string LeftBracket = "[";
        const string RightBracket = "]";
        const string Space = " ";

        [SerializeField]
        Dropdown _formats = default;
        [SerializeField]
        Text _userLabel = default;
        [SerializeField]
        InputField _challengerInputField = default;
        [SerializeField]
        GameObject _challengeReceivedWindow = default;
        [SerializeField]
        Text _challengedUserText = default;
        [SerializeField]
        Text _challengedFormatText = default;
        [SerializeField]
        GameObject _challengeButton = default;
        [SerializeField]
        GameObject _cancelButton = default;
        [SerializeField]
        InputField _teamInput = default;

        public override void EnterState(ShowdownClient context)
        {
            base.EnterState(context);
            PopulateFormats(context.FormatList);
            SetUserLabel(context.Username);
        }

        public override void ReceiveMessage(ServerMessage message)
        {
            ServerMessageData[] payload = message.Payload;
            for (int i = 0; i < payload.Length; i++)
            {
				ServerMessageData serverMessageData = payload[i];
				if (serverMessageData.Type == MessageDataType.UpdateChallenges)
					UpdateChallenges(serverMessageData.Data);
				else if (serverMessageData.Type == MessageDataType.Init && serverMessageData.Data[0] == "battle")
					_context.GoToBattle();
            }
        }

        public void PlayLadder()
        {
            string formatId = CurrentFormatId();
            string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Search, formatId);
            WebsocketConnection.Instance.Send(command);
        }

        public void Logout()
        {
            string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Logout);
            WebsocketConnection.Instance.Send(command);
            _context.GoToLogin();
        }

        public void ChallengeUser()
        {
            if (!string.IsNullOrEmpty(_challengerInputField.text))
            {
                string formatId = CurrentFormatId();
                string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Challenge, _challengerInputField.text, formatId);
                WebsocketConnection.Instance.Send(command);
                _challengeButton.SetActive(false);
                _cancelButton.SetActive(true);
            }
        }

        public void CancelChallenge()
        {
            string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.CancelChallenge, _challengerInputField.text);
            WebsocketConnection.Instance.Send(command);
            _challengeButton.SetActive(true);
            _cancelButton.SetActive(false);
        }

        public void AcceptChallenge()
        {
            string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Accept, _challengedUserText.text);
            WebsocketConnection.Instance.Send(command);
            _challengeReceivedWindow.SetActive(false);
        }

        public void DeclineChallenge()
        {
            string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Reject, _challengedUserText.text);
            WebsocketConnection.Instance.Send(command);
            _challengeReceivedWindow.SetActive(false);
        }

        public void UploadTeam()
        {
            if (!string.IsNullOrEmpty(_teamInput.text))
            {
                string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.UploadTeam, _teamInput.text);
                WebsocketConnection.Instance.Send(command);
            }
        }

        void UpdateChallenges(string[] data)
        {
            JSONNode json = JSON.Parse(data[0]);
            JSONObject challenges = json["challengesFrom"].AsObject;
            if (challenges != null)
            {
                string[] keys = challenges.Keys;
                if (keys.Length > 0)
                {
                    _challengeReceivedWindow.SetActive(true);
                    _challengedUserText.text = keys[0];
                    _challengedFormatText.text = challenges[keys[0]];
                }
            }
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
