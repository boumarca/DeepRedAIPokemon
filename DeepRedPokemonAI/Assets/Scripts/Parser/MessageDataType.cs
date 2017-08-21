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

    static BidirectionalMapping<string, int> _map;
    public static BidirectionalMapping<string, int> Map
    {
        get
        {
            if (_map == null)
                InitMapping();
            return _map;
        }
    }

    static void InitMapping()
    {
        _map = new BidirectionalMapping<string, int>();
        _map.Add("\n", (int)DataType.Empty);
        _map.Add("challstr", (int)DataType.ChallStr);
        _map.Add("queryresponse", (int)DataType.QueryResponse);
        _map.Add("updateuser", (int)DataType.UpdateUser);
        _map.Add("formats", (int)DataType.Formats);
        _map.Add("trn", (int)DataType.Trn);
    }
}