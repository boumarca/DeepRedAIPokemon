using com.MAB.Utils;

public static class MessageDataType
{
    public enum DataType
    {
        Empty,
        ChallStr,
        QueryResponse,
        UpdateUser,
        Formats,
        Trn
    }

    static BidirectionalMapping<string, int> _Map;
    public static BidirectionalMapping<string, int> Map
    {
        get
        {
            if (_Map == null)
                InitMapping();
            return _Map;
        }
    }

    static void InitMapping()
    {
        _Map = new BidirectionalMapping<string, int>();
        _Map.Add("\n", (int)DataType.Empty);
        _Map.Add("challstr", (int)DataType.ChallStr);
        _Map.Add("queryresponse", (int)DataType.QueryResponse);
        _Map.Add("updateuser", (int)DataType.UpdateUser);
        _Map.Add("formats", (int)DataType.Formats);
        _Map.Add("trn", (int)DataType.Trn);
    }
}