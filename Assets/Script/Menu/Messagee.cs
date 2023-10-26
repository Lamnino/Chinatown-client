

[System.Serializable]
public class Messagee
{
    private string Message;
    private int id;
    private string Time;
    private string namePlayer;
    public string message
    {
        get { return Message; }
        set { Message = value;  }
    }
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string time
    {
        get { return Time; }
        set { Time = value; }
    }
    public string NamePlayer
    {
        get { return namePlayer; }
        set { namePlayer = value; }
    }


}

