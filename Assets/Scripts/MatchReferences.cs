public class MatchReference {
    private static string _matchID;
    private static string _host;
    private static string _time;

    public static string host {
        get { return _host; }
        set { _host = value; }
    }

    public static string time {
        get { return _time; }
        set { _time = value; }
    }

    public static string matchID {
        get { return _matchID; }
        set { _matchID = value; }
    }
}
