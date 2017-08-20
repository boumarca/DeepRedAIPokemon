using com.MAB.Utils;

public static class MessageDataType
{
    public enum DataType
    {
        Empty,
        ChallStr,
        QueryResponse,
        UpdateUser,
        Formats
    }

    static BidirectionalMapping<string, DataType> _Map;
    public static BidirectionalMapping<string, DataType> Map
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
        _Map = new BidirectionalMapping<string, DataType>();
        _Map.Add("\n", DataType.Empty);
        _Map.Add("challstr", DataType.ChallStr);
        _Map.Add("queryresponse", DataType.QueryResponse);
        _Map.Add("updateuser", DataType.UpdateUser);
        _Map.Add("formats", DataType.Formats);
    }
}