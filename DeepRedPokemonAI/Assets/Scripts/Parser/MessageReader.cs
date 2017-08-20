using System;

namespace DeepRedAI.Parser
{  
    public static class MessageReader
    {
        static readonly char[] LineFeedDelimiter = { '\n' };
        static readonly char[] MessageDelimiter = { '|' };
        const string RoomIdPrefix = ">";

        public static ServerMessage Parse(string message)
        {
            if (string.IsNullOrEmpty(message))
                return null;

            string[] lines = message.Split(LineFeedDelimiter);
            if (lines.Length == 0)
                return null;

            string roomId = ParseRoomId(lines[0]);
            int startIndex = string.IsNullOrEmpty(roomId) ? 0 : 1;
            ServerMessageData[] payload = new ServerMessageData[lines.Length - startIndex];
            int payloadIndex = 0;
            for (int i = startIndex; i < lines.Length; i++)
            {
                ServerMessageData smd = ParsePayload(lines[i]);
                if (smd != null)
                {
                    payload[payloadIndex] = smd;
                    payloadIndex++;
                }
            }
            return new ServerMessage(roomId, payload);
        }

        static string ParseRoomId(string message)
        {
            string roomId = string.Empty;
            if (message.StartsWith(RoomIdPrefix))
                roomId = message.Substring(1);
            return roomId;
        }

        static ServerMessageData ParsePayload(string line)
        {
            string[] splits = line.Split(MessageDelimiter);
            if (splits.Length == 0)
                return null;

            MessageDataType.DataType type = MessageDataType.DataType.Empty;
            string[] data = null;
            if (splits.Length > 1)
            {
                type = MessageDataType.Map[splits[1]];
                data = new string[splits.Length - 2];
                Array.Copy(splits, 2, data, 0, data.Length);
            }

            return new ServerMessageData(type, data);
        }
    }
}
