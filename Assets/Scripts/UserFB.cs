public class UserFB {
    string _userID;
    string _username;

    public UserFB(string userID, string username) {
        _userID = userID;
        _username = username;
    }

    public string userID {
        get { return _userID; }
        set { _userID = value; }
    }

    public string username {
        get { return _username; }
        set { _username = value; }
    }
}
