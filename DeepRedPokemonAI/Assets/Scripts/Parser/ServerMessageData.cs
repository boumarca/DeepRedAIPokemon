
using System.Text;

namespace DeepRedAI.Parser
{
    public class ServerMessageData
    {
        public MessageDataType.DataType Type { get; private set; }
        public string[] Data { get; private set; }

        public ServerMessageData(MessageDataType.DataType type, string[] data)
        {
            Type = type;
            Data = data;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append('|');
            s.Append(MessageDataType.Map[Type]);
            s.Append('|');
            s.Append(string.Join("|", Data));
            return s.ToString();
        }
    }
}