using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.UI;

namespace DeepRedAI.Showdown
{
	public class ClientBattleState : ClientState
	{
		[SerializeField]
		Text _roomIdText = default;
		public override void ReceiveMessage(ServerMessage message)
		{
			_roomIdText.text = _context.RoomId;
		}
	}
}
