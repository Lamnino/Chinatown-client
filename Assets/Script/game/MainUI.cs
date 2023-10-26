using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class MainUI : MonoBehaviour
{
    public static MainUI instance;
    public GameObject Loading;
    RoomManager room;
    [SerializeField] private TextMeshProUGUI[] NameTMP;
    [SerializeField] private GameObject[] ready;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartGame();
        room = gamemng.instance.room;
    }
    
    public void StartGame()
    {
        if (gamemng.instance.room.Players.Count > 1)
        {
            foreach (PlayerInRomm pl in gamemng.instance.room.Players)
            {
                    NameTMP[RoomManager.index(pl.id)].text = pl.Name;
                    if (!pl.Ready)
                    {
                        ready[RoomManager.index(pl.id)].SetActive(true);
                    }
                    DisPlayCard(pl, RoomManager.index(pl.id));
            }
        }
    }
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject[] colorObj;
    [SerializeField] private GameObject CommitColorBtn;
    public void ChoseColor(byte[] color)
    {
        CommitColorBtn.SetActive(false);
        for (int i = 0; i<5; i++)
        {
            if (color[i] == 0)
                colorObj[i].SetActive(false);
        }
    }
    public void Readymember(ushort id,byte color,ushort idmanager)
    {
        Loading.SetActive(false);
        if (color == room.You.Color())
        {
            CommitColorBtn.SetActive(false);
            colorObj[color-1].SetActive(false);
        }
        if (id != room.You.id)
        {
            int i = RoomManager.index(id);
            if (i != -1)
            {
                ready[i].SetActive(true);
            }
        }
        if (room.You.id == idmanager && room.Players.Count>2)
            startBtn.SetActive(true);
        room.setIdManger(idmanager);
    }
    public void NewmemberJoinroom(string json)
    {
        PlayerInRomm newmember = JsonConvert.DeserializeObject<PlayerInRomm>(json);
        room.AddnewMember(newmember);
        int i = room.Players.Count-2;
        NameTMP[i].text = newmember.Name;
    }
    [SerializeField] ContainCard[] CardContent;
    [SerializeField] Image[] PlayerColor;
    [SerializeField] Transform groundContent;
    [SerializeField] GameObject cimmitground;
    public void Startgame(byte[] color)
    {
        Loading.SetActive(false);
        int i = 0;
        foreach(var m in room.Players)
        {
            m.setColor(color[i]);
            m.StartCard();
            PlayerColor[RoomManager.index(m.id)].color = Ground.GetColor(m.Color(), 255);
            i++;
        }
        BankUI.instance.UIWhenStartGame();
    }
        public void SeperateCard(string ground, string card, byte[] color)
    {
        cimmitground.SetActive(true);
        List<byte> Card = JsonConvert.DeserializeObject<List<byte>>(card);
        Debug.Log(card);
        byte[] grounds = JsonConvert.DeserializeObject<byte[]>(ground);
        gamemng.instance.numGroundIsChose = grounds.Length-2;
        gamemng.instance.state = "chose ground";
        Debug.Log(Card.Count);
        room.You.UpdateCard(Card);
        for (int i= 0; i < groundContent.childCount; i++)
        {
            Ground grscp = groundContent.GetChild(i).GetComponent<Ground>();
            bool isHave = false;
            foreach (var gr in grounds)
            {
                if (gr == grscp.numGround)
                {
                    isHave = true;
                    Ground.listGround.Add(gr);
                    break;
                }
            }
            if (!isHave) grscp.hideGround();
        }
        DisPlayCard(room.You,4);
    }
    private void DisPlayCard(PlayerInRomm player, int index)
    {
        for (int i = 0; i < 12; i++)
        {
            if (player.Card()[i] != 0)
            {
                CardContent[index].listCard[i].gameObject.SetActive(true);
                CardContent[index].listCard[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = player.Card()[i].ToString();
            }
            else
            {
                CardContent[index].listCard[i].gameObject.SetActive(false);
            }
        }
            //PlayerColor[index].color = Ground.GetColor(player.Color(),255);
    }
    [SerializeField] private GameObject HistoryBtn;
    public void CommitGround(string jsonGround,string jsonCard)
    {
        for (int i= 0; i<groundContent.childCount; i++)
        {
            groundContent.GetChild(i).GetComponent<Ground>().displayGround();
        }
        List<byte>[] grounds = JsonConvert.DeserializeObject<List<byte>[]>(jsonGround);
        List<byte>[] cards = JsonConvert.DeserializeObject<List<byte>[]>(jsonCard);
        for (int i=0; i < room.Players.Count; i++)
        {
            for(int j =0; j< grounds[i].Count; j++)
            {
                groundContent.GetChild(grounds[i][j]-1).GetComponent<Ground>().SetColor(room.Players[i].Color());
                room.Players[i].AddGround(grounds[i][j]);
            }
            int d = RoomManager.index(room.Players[i].id);
            if (RoomManager.index(room.Players[i].id) != -1 && room.Players[i] != room.You)
            {
                room.Players[i].UpdateCard1(cards[i]);
                DisPlayCard(room.Players[i], d);
            }
        }
        HistoryBtn.SetActive(true);
    }
    [SerializeField] GameObject AnnoceMainUI;
    //[SerializeField] GameObject AnnocExchangeitemSuccess;
    //[SerializeField] GameObject AnnocExchangeitemrefuse;
    public void ReplyReplyExchangeItemRequest(byte idrequest, bool result)
    {
        ExchangeImform exchangerequest = gamemng.instance.exchangeItems.Find(item => item.Id == idrequest);
        PlayerInRomm recieive = room.Players.Find(item => item.id == exchangerequest.IdReceive);
        PlayerInRomm sent = room.Players.Find(item => item.id == exchangerequest.IdSent);
        if (result) 
        {
             NewHistoryExchange(exchangerequest);
            if (gamemng.instance.room.You.id == exchangerequest.IdReceive)
            {
                Loading.SetActive(false);
                MainUI.instance.Displayerannouce("Success to exchange items", false);
                BankUI.instance.ExchangeItemUi.SetActive(false);
                Destroy(BankManager.ExchangerequestBtnToDelete);
            }
            else
            if (gamemng.instance.room.You.id == exchangerequest.IdSent)
            {
                //AnnocExchangeitemSuccess.SetActive(true);
                Displayerannouce($"{gamemng.instance.room.Players.Find(item => item.id == exchangerequest.IdReceive).Name} agreed exchange item", false);
            }
            //success
            //set ground
            if (exchangerequest.GrousdChose.Count > 0)
            {
                foreach (byte gr in exchangerequest.GrousdChose)
                {
                    //SetUI
                    Ground ground = groundContent.GetChild(gr - 1).GetComponent<Ground>();
                    ground.SetColor(recieive.Color());
                    recieive.AddGround(gr);
                    sent.removeground(gr);
                }
            }
            if (exchangerequest.GrousdGet.Count > 0)
            {
                foreach (byte gr in exchangerequest.GrousdGet)
                {
                    //SetUI
                    Ground ground = groundContent.GetChild(gr - 1).GetComponent<Ground>();
                    ground.SetColor(sent.Color());
                    sent.AddGround(gr);
                    recieive.removeground(gr);
                }
            }
            //card store
            for (byte i = 0; i < 12; i++)
            {
                if (exchangerequest.CardChose[i] != 0)
                {
                    recieive.AddCard(i);
                    sent.removecard(i);
                }
                if (exchangerequest.CardGet[i] != 0)
                {
                    sent.AddCard(i);
                    recieive.removecard(i);
                }
            }
                DisPlayCard(sent, RoomManager.index(sent.id));
                DisPlayCard(recieive, RoomManager.index(recieive.id));
            if (exchangerequest.Money != 0)
                {
                recieive.addCash(exchangerequest.Money);
                sent.addCash(-exchangerequest.Money);
            }
            if (exchangerequest.Money != 0)
            {
                recieive.addCash(-exchangerequest.MoneyGet);
                sent.addCash(-exchangerequest.MoneyGet);
            }
        }
        else
        {
            if (gamemng.instance.playerinf.Id ==  exchangerequest.IdSent)
            {
                Displayerannouce("you were refused exchange item", true);
            }
            else
            {
                    BankUI.instance.ClearExchangeItemUI();
                    Destroy(BankManager.ExchangerequestBtnToDelete);
                BankUI.instance.ExchangeItemUi.SetActive(false);
                Loading.SetActive(false);
                Displayerannouce("refuse exchange item or item were used", true);
            }
                gamemng.instance.exchangeItems.Remove(exchangerequest);
        }
    }
    [SerializeField] private Transform historyExchangepref;
    [SerializeField] private Transform contentHistoryExchange;
    public void NewHistoryExchange(ExchangeImform exchangerequest)
    {
        Transform newHistory = Instantiate(historyExchangepref,contentHistoryExchange);
        newHistory.SetAsFirstSibling();
        newHistory.GetChild(0).GetComponent<TextMeshProUGUI>().text = gamemng.instance.room.Players.Find(item => item.id == exchangerequest.IdSent).Name;
        newHistory.GetChild(1).GetComponent<TextMeshProUGUI>().text = gamemng.instance.room.Players.Find(item => item.id == exchangerequest.IdReceive).Name;
        newHistory.GetComponent<Button>().AddEventListener(exchangerequest, newHistory.gameObject, BankUI.instance.HistoryExchangeClick, SetHistoryToChangeColor);
    }
    public void SetHistoryToChangeColor(GameObject history)
    {
        history.GetComponent<Image>().color = new Color32(60, 60, 60, 255);
        BankUI.instance.historyObj = history;
    }
    public void actionExchangeItem(string json)
    {
        ExchangeImform exchangerequest = JsonConvert.DeserializeObject<ExchangeImform>(json);
        PlayerInRomm recieive = room.Players.Find(item => item.id == exchangerequest.IdReceive);
        PlayerInRomm sent = room.Players.Find(item => item.id == exchangerequest.IdSent);
         NewHistoryExchange(exchangerequest);
        if (exchangerequest.GrousdChose.Count > 0)
        {
            foreach (byte gr in exchangerequest.GrousdChose)
            {
                //SetUI
                Ground ground = groundContent.GetChild(gr - 1).GetComponent<Ground>();
                ground.SetColor(recieive.Color());
                recieive.AddGround(gr);
                sent.removeground(gr);
            }
        }
        if (exchangerequest.GrousdGet.Count > 0)
        {
            foreach (byte gr in exchangerequest.GrousdGet)
            {
                //SetUI
                Ground ground = groundContent.GetChild(gr - 1).GetComponent<Ground>();
                ground.SetColor(sent.Color());
                sent.AddGround(gr);
                recieive.removeground(gr);
            }
        }
        //card store
        for (byte i = 0; i < 12; i++)
        {
            if (exchangerequest.CardChose[i] != 0)
            {
                recieive.AddCard(i);
                sent.removecard(i);
            }
            if (exchangerequest.CardGet[i] != 0)
            {
                sent.AddCard(i);
                recieive.removecard(i);
            }
        }
        DisPlayCard(sent, RoomManager.index(sent.id));
        DisPlayCard(recieive, RoomManager.index(recieive.id));
        if (exchangerequest.Money != 0)
        {
            recieive.addCash(exchangerequest.Money);
            sent.addCash(-exchangerequest.Money);
        }
        if (exchangerequest.Money != 0)
        {
            recieive.addCash(-exchangerequest.MoneyGet);
            sent.addCash(-exchangerequest.MoneyGet);
        }
    }
    [SerializeField] private TextMeshProUGUI TextAnnounce;
    [SerializeField] private TextMeshProUGUI warnText;
    [SerializeField] private GameObject WarnAnnoce;
    [SerializeField] private GameObject GreenAnnoce;
    public void Displayerannouce(string announc, bool warn)
    {
        AnnoceMainUI.SetActive(true);
        GreenAnnoce.SetActive(!warn);
        WarnAnnoce.SetActive(warn);
        if (warn) warnText.text = announc;
        else
        TextAnnounce.text = announc;
    }
}
