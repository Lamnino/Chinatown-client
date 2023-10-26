using System.Collections.Generic;
using Newtonsoft.Json;
public class RoomManager 
{
    static private List<PlayerInRomm> players = new List<PlayerInRomm>();
    private static PlayerInRomm you;
    private int idrom;
    static byte year;
    public byte ready;
    private bool isstart = false;
    private ushort idManager;
    public List<LoanDetail> loans = new List<LoanDetail>();
    public RoomManager(int id)
    {
        idrom = id;
    }
    public RoomManager(string json, int id)
    {
        idrom = id;
        players = JsonConvert.DeserializeObject<List<PlayerInRomm>>(json);
        //you = new PlayerInRomm(gamemng.instance.Id, gamemng.instance.playerinf.Id, gamemng.instance.playerinf.NamePlayer,false);
        //players.Add(you);
    }
    public List<PlayerInRomm> Players
    {
        get { return players;  }
        set { }
    }
    public PlayerInRomm You
    {
        get { return you; }
        set { you = value; }
    }
    public int Idroom
    {
        get { return idrom; }
        private set { }
    }
    public static byte Year()
    {
        return year;
    }
    public void increaseYear()
    {
        year++;
    }
    public void start()
    {
        year = 1;
        isstart = true;
    }
    public bool IsStart()
    {
        return isstart;
    }
    public ushort IdManager()
    {
        return idManager;
    }
    public void setIdManger(ushort id)
    {
        idManager = id;
    }
    public void AddnewMember(PlayerInRomm member)
    {
        players.Add(member);
    }
 
    public static int index(ushort id)
    {
        if (id == you.id) return 4;
        int d = 0;
        foreach(var m in players)
        {
                if (m.id == id) return d;
                if (m.id != you.id)
                d++;
        }
        return -1;
    }
    

}
