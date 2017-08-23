using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DeepRedAI.Parser
{
    public static class MessageWriter
    {
        const string CommandPrefix = "/";
        const string ArgsDelimiter = ",";
        const string Space = " ";

        public static string WriteMessage(string roomId, string command, params string[] args)
        {
            StringBuilder s = new StringBuilder();
            s.Append(roomId);
            s.Append(ServerMessageData.Delimiter);
            s.Append(CommandPrefix);
            s.Append(command);
            s.Append(Space);
            s.Append(string.Join(ArgsDelimiter, args));
            return s.ToString();
        }
    }
}
