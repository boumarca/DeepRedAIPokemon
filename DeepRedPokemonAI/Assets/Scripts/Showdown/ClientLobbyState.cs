using System.Collections;
using System.Collections.Generic;
using DeepRedAI.Parser;
using UnityEngine;

namespace DeepRedAI.Showdown
{
    public class ClientLobbyState : ClientState
    {
        [SerializeField]
        LobbyBattleModule _battleModule;
        [SerializeField]
        LobbyLoginModule _loginModule;
        
        public override void EnterState(ShowdownClient context)
        {
            base.EnterState(context);
            _battleModule.PopulateFormats(context.FormatList);
            _loginModule.SetUserLabel(context.Username);
        }

        public override void ReceiveMessage(ServerMessage message)
        {
        }

        public void PlayLadder()
        {
            string formatId = _battleModule.CurrentFormatId();
            Debug.Log(formatId);
        }
    }
}
