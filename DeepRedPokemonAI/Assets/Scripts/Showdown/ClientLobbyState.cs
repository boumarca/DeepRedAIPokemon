using System.Collections;
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
        Dropdown _formats;
        [SerializeField]
        Text _userLabel;
        [SerializeField]
        InputField _challengerInputField;
        [SerializeField]
        GameObject challengeReceivedWindow;
        [SerializeField]
        Text challengedUserText;
        [SerializeField]
        Text challengedFormatText;
        [SerializeField]
        GameObject challengeButton;
        [SerializeField]
        GameObject cancelButton;
        [SerializeField]
        InputField teamInput;

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
                if (payload[i].Type == MessageDataType.UpdateChallenges)
                    UpdateChallenges(payload[i].Data);
            }
        }

        public void PlayLadder()
        {
            string formatId = CurrentFormatId();
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Search, formatId);
            WebsocketConnection.Instance.Send(command);
        }

        public void Logout()
        {
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Logout);
            WebsocketConnection.Instance.Send(command);
            _context.GoToLogin();
        }

        public void ChallengeUser()
        {
            if (!string.IsNullOrEmpty(_challengerInputField.text))
            {
                string formatId = CurrentFormatId();
                string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Challenge, _challengerInputField.text, formatId);
                WebsocketConnection.Instance.Send(command);
                challengeButton.SetActive(false);
                cancelButton.SetActive(true);
            }
        }

        public void CancelChallenge()
        {
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.CancelChallenge, _challengerInputField.text);
            WebsocketConnection.Instance.Send(command);
            challengeButton.SetActive(true);
            cancelButton.SetActive(false);
        }

        public void AcceptChallenge()
        {
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Accept, challengedUserText.text);
            WebsocketConnection.Instance.Send(command);
            challengeReceivedWindow.SetActive(false);
        }

        public void DeclineChallenge()
        {
            string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.Reject, challengedUserText.text);
            WebsocketConnection.Instance.Send(command);
            challengeReceivedWindow.SetActive(false);
        }

        public void UploadTeam()
        {
            if (!string.IsNullOrEmpty(teamInput.text))
            {
                string command = MessageWriter.WriteMessage(string.Empty, MessageDataType.UploadTeam, teamInput.text);
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
                    challengeReceivedWindow.SetActive(true);
                    challengedUserText.text = keys[0];
                    challengedFormatText.text = challenges[keys[0]];
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
