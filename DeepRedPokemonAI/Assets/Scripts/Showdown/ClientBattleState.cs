using com.MAB.Web;
using DeepRedAI.Parser;
using UnityEngine;
using UnityEngine.UI;

namespace DeepRedAI.Showdown
{
	public class ClientBattleState : ClientState
	{
		[SerializeField]
		Text _roomIdText = default;

		ChoiceRequest _choiceRequest;

		public override void ReceiveMessage(ServerMessage message)
		{
			_roomIdText.text = _context.RoomId;
			ServerMessageData[] payload = message.Payload;
			for (int i = 0; i < payload.Length; i++)
			{
				ServerMessageData serverMessageData = payload[i];
				if (serverMessageData.Type == MessageDataType.Deinit)
				{
					_context.GoToLobby();
				}
				else if (serverMessageData.Type == MessageDataType.Request && !string.IsNullOrEmpty(serverMessageData.Data[0]))
				{
					_choiceRequest = ChoiceRequest.Parse(serverMessageData.Data[0]);
				}
			}
		}

		public void LeaveRoom()
		{
			string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Leave);
			WebsocketConnection.Instance.Send(command);
		}

		public void ForfeitBattle()
		{
			string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Forfeit);
			WebsocketConnection.Instance.Send(command);
			LeaveRoom();
		}
	}
}
