using System.Collections.Generic;
[System.Serializable]
public class PlayerInRomm 
{
    private ushort Id;
    private int Idserver;
    private string name;
    public bool Ready;

    private List<byte> ground = new List<byte>();
    private List<byte> card = new List<byte>();
    //private int money = 50000;
    private int cash = 50000;
    private int loan = 0;
    private int deposite = 0;
    private byte color = 0;  // 1: red; 2:orange; 3: silver; 4: Grenn; 5: Blue


    public PlayerInRomm(ushort Id, int Idserver, string name,bool ready)
    {
        this.Id = Id;
        this.Idserver = Idserver;
        this.name = name;
        this.Ready = ready;
        StartCard();
    }
    public void StartCard()
    {
        for (int i = 0; i < 12; i++) card.Add(0);
    }
    public List<byte> Ground()
    {
        return ground;
    }
    public void AddGround(byte groundd)
    {
        if (!ground.Contains(groundd))
        ground.Add(groundd);
    }
    public void removeground(byte gr)
    {
        ground.Remove(gr);
    }
    public byte Color()
    {
        return color;
    }
    public void setColor(byte colorr)
    {
        color = colorr;
    }
   
    public List<byte> Card()
    {
        return card;
    }
    public void UpdateCard(List<byte> card)
    {
       for (int i=0; i<card.Count;i++)
        {
                this.card[card[i]]++;
        }
    }
    public void UpdateCard1(List<byte> card)
    {
        for (int i =0; i<12; i++)
        {
            if (card[i]!= 0)
            {
                this.card[i] += card[i];
            }
        }
    }
    public void AddCard(byte c)
    {
        card[c]++;
    }
    public void removecard(byte c)
    {
        if (card[c] > 0) card[c]--;
    }

    public int Cash()
    {
        return cash;
    }
    public void addCash(int amount)
    {
        cash += amount;
    }
    public int Loan()
    {
        return loan;
    }
    public void addloan(int amount)
    {
        loan += amount;
    }
    public void adddeposite(int amount)
    {
        deposite += amount;
    }
    public int Deposite()
    {
        return deposite;
    }
    public ushort id
    {
        get { return Id; }
        set { Id = ushort.Parse(value.ToString());  }
    }
    public int idserver
    {
        get { return Idserver; }
        set { Idserver = ushort.Parse(value.ToString()); }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

      
}
