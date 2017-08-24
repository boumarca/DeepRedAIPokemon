using com.MAB.Web;
using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace DeepRedAI.Showdown
{
    public class ShowdownClient : MonoBehaviour
    {
        const string LeftBracket = "[";
        static readonly char[] FormatsDelimiters = new char[] { ',' };

        [Header("States")]
        [SerializeField]
        ClientState _loginState;
        [SerializeField]
        ClientState _lobbyState;

        ClientState _state;


        public List<string> FormatList { get; private set; }
        public string Username { get; set; }

        void Awake()
        {
            Assert.IsNotNull(_loginState, "LoginState is null");
            Assert.IsNotNull(_loginState, "LobbyState is null");
        }

        void Start()
        {
            WebsocketConnection.Instance.OnMessageReceived += MessageReceived;
            ChangeState(_loginState);
        }

        public void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            ServerMessage m = MessageReader.Parse(e.Message);
            if(m != null)
                _state.ReceiveMessage(m);
        }

        void ChangeState(ClientState newState)
        {
            if (_state != null)
                _state.LeaveState();

            _state = newState;

            if (_state != null)
                _state.EnterState(this);
        }

        public void GoToLobby()
        {
            ChangeState(_lobbyState);
        }

        public void PopulateFormatList(string[] data)
        {
            FormatList = new List<string>(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].StartsWith(LeftBracket))
                {
                    string[] splits = data[i].Split(FormatsDelimiters);
                    FormatList.Add(splits[0]);
                }
            }

        }
    }
}
