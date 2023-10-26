using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Mainmanager : MonoBehaviour
{
    public static Mainmanager instance; 
    gamemng gamemanager;
    RoomManager room;
    GameObject mainui;
    public Sprite[] spritecard;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gamemanager = gamemng.instance;
        room = gamemng.instance.room;
    }
    public void ChoseColorBtn()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ChoseColor);
        message.AddInt(room.Idroom);
        NetworkManager.Singleton.client.Send(message, true);
    }
    [MessageHandler((ushort)ServerToClient.ChoseColor)]
    private static void ReciveresultChoseColor(Message message)
    {
        MainUI.instance.ChoseColor(message.GetBytes());
    }
    [SerializeField] GameObject yourReady;
    [SerializeField] private GameObject[] colorObj;
    [SerializeField] private GameObject ColorBtn;
    [SerializeField] private GameObject ColorIschose;
    public void readyBtn()
    {
        if (gamemng.instance.room.You.Color() != 0)
        {
            MainUI.instance.Loading.SetActive(true);
            if (colorObj[gamemng.instance.room.You.Color() - 1].activeSelf)
            {
                gamemng.instance.room.You.Ready = true;
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.readyMember);
                message.AddInt(room.Idroom);
                message.AddByte(gamemng.instance.room.You.Color());
                NetworkManager.Singleton.client.Send(message, true);
                gamemng.instance.state = "Ready";
            }
            else
            {
                ColorBtn.SetActive(true);
                yourReady.SetActive(false);
                MainUI.instance.Displayerannouce("Color iwas chosen by other member", true);
                MainUI.instance.Loading.SetActive(false);
            }
        }
        else
        {
            MainUI.instance.Displayerannouce("Please chose your color", true);
        }
       
    }
    [MessageHandler((ushort)ServerToClient.readyMember)]
    private static void receiveReadyMember(Message message)
    {
        MainUI.instance.Readymember(message.GetUShort(),message.GetByte(),message.GetUShort());
    }
    [MessageHandler((ushort)ServerToClient.NewmemberJoinRoom)]
    private static void receiveNewMemberjoinRoom(Message message)
    {
        MainUI.instance.NewmemberJoinroom(message.GetString());
    }
    public void StartBtn()
    {
        MainUI.instance.Loading.SetActive(true);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.StartGame);
        message.AddInt(room.Idroom);
        NetworkManager.Singleton.client.Send(message, true);
    }
    [MessageHandler((ushort)ServerToClient.StartGame)]
    private static void RecieveStartGame(Message message)
    {
        MainUI.instance.Startgame(message.GetBytes());
    }
    [MessageHandler((ushort)ServerToClient.seperateCrad)]
    private static void RecieveCardSeperate(Message message)
    {
        MainUI.instance.SeperateCard(message.GetString(), message.GetString(), message.GetBytes());
    }
    [SerializeField] private GameObject CommitGroundBtn;
    [SerializeField] private Transform contentGround;
    public void CommitGround()
    {
        if (Ground.GroundChose.Count < gamemng.instance.numGroundIsChose)
        {
            MainUI.instance.Displayerannouce("Please chose enough ground", true);
        }
        else
        {
            CommitGroundBtn.SetActive(false);
            gamemng.instance.state = "none action";
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.commitGround);
            message.AddString(Ground.Groundchose());
            message.AddString(Ground.GroudLost());
            message.AddInt(room.Idroom);
            NetworkManager.Singleton.client.Send(message, true);
        }
        foreach(var grc in Ground.GroundChose)
        {
            contentGround.GetChild(grc - 1).GetComponent<Ground>().owner = true;
        }
        Ground.GroundChose.Clear();
        Ground.listGround.Clear();
    }
    [MessageHandler((ushort)ServerToClient.commitGround)]
    private static void RecieveResultCommitGround(Message message)
    {
        MainUI.instance.CommitGround(message.GetString(),message.GetString());
    }
    [MessageHandler((ushort)ServerToClient.actionExchangeItem)]
    private static void ReceiveactionExchangeItem(Message message)
    {
        MainUI.instance.actionExchangeItem(message.GetString());
    }
        IEnumerator DisplayAnnounc(GameObject obj)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(6f);
            obj.SetActive(false);
        }
}
