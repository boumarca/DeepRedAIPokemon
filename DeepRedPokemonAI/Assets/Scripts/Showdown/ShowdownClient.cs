using com.MAB.Web;
using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.Assertions;

namespace DeepRedAI.Showdown
{
    public class ShowdownClient : MonoBehaviour
    {
        [Header("States")]
        [SerializeField]
        ClientState _loginState;

        ClientState _state;

        void Awake()
        {
            Assert.IsNotNull(_loginState, "LoginState is null");
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
                _state.ReceiveMessage(this, m);
        }

        void ChangeState(ClientState newState)
        {
            if (_state != null)
                _state.LeaveState();

            _state = newState;

            if (_state != null)
                _state.EnterState();
        }
    }
}
