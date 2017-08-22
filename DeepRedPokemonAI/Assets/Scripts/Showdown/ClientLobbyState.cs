﻿using System.Collections;
using System.Collections.Generic;
using DeepRedAI.Parser;
using UnityEngine;

namespace DeepRedAI.Showdown
{
    public class ClientLobbyState : ClientState
    {
        [SerializeField]
        LobbyBattleModule _battleModule;

        public override void EnterState(ShowdownClient context)
        {
            base.EnterState(context);
            _battleModule.PopulateFormats(context.FormatList);

        }

        public override void ReceiveMessage(ShowdownClient context, ServerMessage message)
        {
        }
    }
}
