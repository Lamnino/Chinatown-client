[System.Serializable]
public class Conversation
{
    private string Id;
    private string LastMessage;
    private char Type;
    private string NameRoom;
    private string State;
    public string id
    {
        get { return Id; }
        set { Id = value; }
    }
    public string lastMessage
    {
        get { return LastMessage; }
        set { LastMessage = value; }
    }
    public char type
    {
        get { return Type; }
        set { Type = value; }
    }
    public string nameRoom
    {
        get { return NameRoom; }
        set { NameRoom = value; }
    }
    public string state
    {
        get { return State; }
        set { State = value; }
    }
}
