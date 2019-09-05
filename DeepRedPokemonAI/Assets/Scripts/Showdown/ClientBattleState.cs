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
		[SerializeField]
		Text _player1Text = default;
		[SerializeField]
		Text _player2Text = default;

		ChoiceRequest _choiceRequest;

		string _player1;
		string _player2;

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
				else if (serverMessageData.Type == MessageDataType.Player)
				{
					string playerId = serverMessageData.Data[0];
					if (playerId == "p1")
					{
						_player1 = serverMessageData.Data[1];
						_player1Text.text = _player1;
					}
					else if (playerId == "p2")
					{
						_player2 = serverMessageData.Data[1];
						_player2Text.text = _player2;
					}
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
