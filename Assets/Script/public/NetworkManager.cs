using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;
using System;
using System.Net;

public enum ClientToServer : ushort
{
    Login = 1,
    Signin,
    JoinRoom,
    AvatarMain,
    NotificationMenu,
    ListMessageInMenu,
    GetChatMessageInMenu,
    SendMessageMenu,
    GetAddFriendRequest,
    AddFriendRequest =10,
    ReplyAddFriendRequest,
    NewmemberJoinRoom,
    PlayGameNow,
    ChoseColor,
    readyMember,
    StartGame,
    seperateCrad,
    commitGround,
    commitechange,
    ReplyExchangeItemRequest =20,
    actionExchangeItem,
    commitLoan,
    ReplyLoan,
    ReLoan,
    Deposite,
}
public enum ServerToClient : ushort
{
    Login = 1,
    Signin,
    JoinRoom,
    AvatarMain,
    NotificationMenu,
    ListMessageInMenu,
    GetChatMessageInMenu,
    SendMessageMenu,
    GetAddFriendRequest,
    AddFriendRequest,
    ReplyAddFriendRequest,
    NewmemberJoinRoom,
    PlayGameNow,
    ChoseColor,
    readyMember,
    StartGame,
    seperateCrad,
    commitGround,
    commitechange,
    ReplyExchangeItemRequest,
    actionExchangeItem,
    commitLoan,
    ReplyLoan,
    ReLoan,
    Deposite,
}
public class NetworkManager : MonoBehaviour
{

    public static NetworkManager singleton;
    public static NetworkManager Singleton
    {
        get => singleton;
        set
        {
            if (singleton == null)     singleton = value;
            else if (singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exist");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        if (singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public Client client { get; private set; }
    [SerializeField] private string ip;
    [SerializeField] private ushort port;
    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogError, Debug.LogWarning, false);

        client = new Client();
        client.Connect($"{ip}:{port}");
        client.Connected += Connected;
        client.ConnectionFailed += FailToConnect;
        client.Disconnected += Disconnectt;
    }

    private void FixedUpdate()
    {
        client.Update();
    }

    private void OnApplicationQuit()
    {
        client.Disconnect();
        client.Connected -= Connected;
        client.ConnectionFailed -= FailToConnect;
        client.Disconnected -= Disconnectt;
    }
    private void Connected(object sender, EventArgs e)
    {
        //DisconnectUI.singleton.Connect();
    }
    private void Disconnectt(object sender, EventArgs e)
    {
        //DisconnectUI.singleton.Disconect();
    }
    private void FailToConnect(object sender, EventArgs e)
    {
        //DisconnectUI.singleton.FailConnect();
        //Debug.Log(Login.resultt);
    }
}
