using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

public class ServerMessageData
{
    public enum DataType
    {
        Empty,
        ChallStr,
        QueryResponse,
        UpdateUser,
        Formats
    }

    public DataType Type { get; private set; }
    public string[] Data { get; private set; }

    public ServerMessageData(DataType type, string[] data)
    {
        Type = type;
        Data = data;
    }

    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        s.Append('|');
        s.Append(Type.ToString().ToLower());
        s.Append('|');
        s.Append(string.Join("|", Data));
        return s.ToString();
    }
}

public static class MessageParser
{
    static readonly char[] LineFeedDelimiter = { '\n' };
    static readonly char[] MessageDelimiter = { '|' };
    const string RoomIdPrefix = ">";

    static readonly Dictionary<string, ServerMessageData.DataType> DataTypeMapping = new Dictionary<string, ServerMessageData.DataType> 
    {
        { "\n", ServerMessageData.DataType.Empty },
        { "challstr", ServerMessageData.DataType.ChallStr },
        { "queryresponse", ServerMessageData.DataType.QueryResponse },
        { "updateuser", ServerMessageData.DataType.UpdateUser },
        { "formats", ServerMessageData.DataType.Formats }
    };

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
            string[] splits = lines[i].Split(MessageDelimiter);
            if (splits.Length == 0)
                continue;
            ServerMessageData.DataType type = ServerMessageData.DataType.Empty;
            string[] data = null;
            if (splits.Length > 1)
            {
                type = DataTypeMapping[splits[1]];
                data = new string[splits.Length - 2];
                Array.Copy(splits, 2, data, 0, data.Length);
            }            
            
            ServerMessageData smd = new ServerMessageData(type, data);
            payload[payloadIndex] = smd;
            payloadIndex++;
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
}
