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

		public override void ReceiveMessage(ServerMessage message)
		{
			_roomIdText.text = _context.RoomId;
			ServerMessageData[] payload = message.Payload;
			for (int i = 0; i < payload.Length; i++)
			{
				ServerMessageData serverMessageData = payload[i];
				if (serverMessageData.Type == MessageDataType.Leave || serverMessageData.Type == MessageDataType.LeaveShort)
				{
					if (serverMessageData.Data.Length > 0)
					{
						//The first character being their rank (users with no rank are represented by a space), and the rest of the string being their username.
						string username = serverMessageData.Data[0].Substring(1); 
						if (_context.IsUser(username))
							_context.GoToLobby();
					}
				}					
			}
		}

		public void LeaveRoom()
		{
			string command = MessageWriter.WriteMessage(_context.RoomId, MessageDataType.Leave);
			WebsocketConnection.Instance.Send(command);
		}
	}
}
