using System.Text;

namespace DeepRedAI.Parser
{
    public class ServerMessageData
    {
        public const string Delimiter = "|";

        public string Type { get; private set; }
        public string[] Data { get; private set; }

        public ServerMessageData(string type, string[] data)
        {
            Type = type;
            Data = data;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(Delimiter);
            s.Append(Type);
            s.Append(Delimiter);
            s.Append(string.Join(Delimiter, Data));
            return s.ToString();
        }
    }
}