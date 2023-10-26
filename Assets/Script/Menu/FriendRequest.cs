[System.Serializable]
public class FriendRequest
{
    private string Id;
    private string Message;
    private string namePlayer;

    public string id
    {
        get { return Id; }
        set { Id = value;  }
    }
    public string message
    {
        get { return Message; }
        set { Message = value; }
    }
    public string NamePlayer
    {
        get { return namePlayer; }
        set { namePlayer = value; }
    }

}
