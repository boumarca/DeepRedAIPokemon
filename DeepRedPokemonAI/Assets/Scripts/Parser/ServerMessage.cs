using System.Text;

namespace DeepRedAI.Parser
{
    public class ServerMessage
    {
        public string RoomId { get; private set; }
        public ServerMessageData[] Payload { get; private set; }

        public ServerMessage(string roomId, ServerMessageData[] payload)
        {
            RoomId = roomId;
            Payload = payload;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            if (!string.IsNullOrEmpty(RoomId))
            {
                s.Append('>');
                s.AppendLine(RoomId);
            }

            for (int i = 0; i < Payload.Length; i++)
            {
                s.Append(Payload[i].ToString());
                if (i < Payload.Length - 1)
                    s.AppendLine();
            }

            return s.ToString();
        }
    }
}
