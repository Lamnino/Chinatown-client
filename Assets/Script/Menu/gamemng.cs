using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class gamemng : MonoBehaviour
{
    public static gamemng instance;
    public ushort Id = 0;
    public PlayerInf playerinf;
    public RoomManager room;//= new RoomManager();
    //menu
    public bool isFetchNotificationsMenu = false;
    public bool isFetchConversationMenu = false;

    //Message
    public int newMessage = 0;
    public bool isgetaddfrie = false;

    public int idOldChat = 0;
    public string NameChat = "";
    public bool stateChat = false;
    public int idChatRoom = 0;

    public string state = "none action";
    public int numGroundIsChose = 0;

    public ushort idWhoYouExChangeItem = 0;
    public List<ExchangeImform> exchangeItems = new List<ExchangeImform>();
    public byte IdRequestExchangeItem = 0;

    public byte idRequestLoan;
    public int IdRePayLoan = 0;

    private void Awake()
    {
        string json1 = "{ \"id\":2,\"idserver\":36,\"Name\":\"Two\",\"Ready\":false}";
        string json2 = "{ \"id\":3,\"idserver\":36,\"Name\":\"three\",\"Ready\":false}";
        playerinf = new PlayerInf(36, "one", 100);
        Id = 1;

        //string json1 = "{ \"id\":1,\"idserver\":36,\"Name\":\"one\",\"Ready\":false}";
        //string json2 = "{ \"id\":3,\"idserver\":36,\"Name\":\"three\",\"Ready\":false}";
        //playerinf = new PlayerInf(36, "two", 100);
        //Id = 2;

        //string json1 = "{ \"id\":1,\"idserver\":36,\"name\":\"one\",\"ready\":false}";
        //string json2 = "{ \"id\":2,\"idserver\":36,\"name\":\"two\",\"ready\":false}";
        //playerinf = new PlayerInf(36, "three", 100);
        //Id = 3;

        instance = this;

        room = new RoomManager(12);
        //room.You = JsonConvert.DeserializeObject<PlayerInRomm>("{ \"id\":1,\"idserver\":36,\"Name\":\"One\",\"Ready\":false}");
        room.You = new PlayerInRomm(Id, 36, playerinf.NamePlayer, false);
        PlayerInRomm player1 = JsonConvert.DeserializeObject<PlayerInRomm>(json1);
        PlayerInRomm player2 = JsonConvert.DeserializeObject<PlayerInRomm>(json2);
        room.Players.Add(player1);
        room.Players.Add(player2);
        room.Players.Add(room.You);
        //room = new RoomManager("[{ \"id\":2,\"idserver\":37,\"Name\":\"Two\",\"Ready\":false},{ \"id\":3,\"idserver\":37,\"Name\":\"Two\",\"Ready\":false}]",12);
        //room = new RoomManager("[{ \"id\":1,\"idserver\":37,\"Name\":\"One\",\"Ready\":false},{ \"id\":3,\"idserver\":37,\"Name\":\"Two\",\"Ready\":false}]",12);
        //room = new RoomManager("[{ \"id\":1,\"idserver\":37,\"Name\":\"One\",\"Ready\":false},{ \"id\":2,\"idserver\":37,\"Name\":\"Two\",\"Ready\":false}]",12);
        //foreach (var m in room.Players)
        //{
        //    m.StartCard();
        //}
        //for (byte i = 1; i < 9; i++)
        //{
        //    room.You.AddGround(i);
        //    player1.AddGround((byte)(i + 9));
        //    player2.AddGround((byte)(i + 9 * 2));
        //}
        //player1.AddCard(2); player1.AddCard(1); player1.AddCard(6); player1.AddCard(6); player1.AddCard(9); player1.AddCard(7);
        //room.You.AddCard(1); room.You.AddCard(1); room.You.AddCard(1); room.You.AddCard(5); room.You.AddCard(6); room.You.AddCard(10);
        //string json1 = "{ \"id\":1,\"idserver\":36,\"Name\":\"one\",\"Ready\":true}";
        //string json2 = "{ \"id\":3,\"idserver\":36,\"Name\":\"three\",\"Ready\":true}";
        //instance = this;
        //playerinf = new PlayerInf(37, "two", 100);
        //room.You = new PlayerInRomm(2, gamemng.instance.playerinf.Id, gamemng.instance.playerinf.NamePlayer, true);
        //PlayerInRomm player1 = JsonConvert.DeserializeObject<PlayerInRomm>(json1);
        //PlayerInRomm player2 = JsonConvert.DeserializeObject<PlayerInRomm>(json2);
        //room.Players.Add(player1);
        //room.Players.Add(room.You);
        //room.Players.Add(player2);
        //foreach (var m in room.Players)
        //{
        //    m.StartCard();
        //}
        //for (byte i = 1; i < 9; i++)
        //{
        //    player1.AddGround(i);
        //    room.You.AddGround((byte)(i + 9));
        //    player2.AddGround((byte)(i + 9 * 2));
        //}
        //player1.AddCard(1); player1.AddCard(1); player1.AddCard(1); player1.AddCard(5); player1.AddCard(6); player1.AddCard(10);
        ////store card of player 2
        //room.You.AddCard(2); room.You.AddCard(1); room.You.AddCard(6); room.You.AddCard(6); room.You.AddCard(9); room.You.AddCard(7);
    }
}
