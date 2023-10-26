using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Riptide;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Singleton;
    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        Getvartar();
    }
    //avatar    
    void Getvartar()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.AvatarMain);
        message.AddInt(gamemng.instance.playerinf.Id);
        message.AddString(gamemng.instance.playerinf.NamePlayer);
        NetworkManager.Singleton.client.Send(message);
    }
    [MessageHandler((ushort)ServerToClient.AvatarMain)]
    public static void ReceiveAvatarMain(Message message)
    {
        UIMenu.Singleton.Avatar(message.GetBytes(), message.GetBool());
    }
    //Notification
    public void NotificationBtnIcon()
    {
        if (!gamemng.instance.isFetchNotificationsMenu)
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.NotificationMenu);
            message.Add(gamemng.instance.playerinf.Id);
            NetworkManager.Singleton.client.Send(message);
            gamemng.instance.isFetchNotificationsMenu = true;
        }
    }
    [MessageHandler((ushort)ServerToClient.NotificationMenu)]
    private static void ReceiveNotifactionMenu(Message message)
    {
        UIMenu.Singleton.NotificationUI(message.GetString());
    }

    // List conversation
    public void MessageIcon()
    {
        if (!gamemng.instance.isFetchConversationMenu || gamemng.instance.newMessage > 0)
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ListMessageInMenu);
            Debug.Log(gamemng.instance.playerinf.Id);
            message.Add(gamemng.instance.playerinf.Id);
            message.Add(0);
            NetworkManager.Singleton.client.Send(message);
            gamemng.instance.isFetchConversationMenu = true;
        }
    }
    [MessageHandler((ushort)ServerToClient.ListMessageInMenu)]
    private static void receiveListConverstionMenu(Message message)
    {
        UIMenu.Singleton.ListConversation(message.GetString());
    }

    // Get message in chat
    public void GetMessageChat()
    {
        int id = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        if (id == gamemng.instance.idOldChat) return;
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.GetChatMessageInMenu);
        message.AddInt(id);
        message.AddInt(0);
        NetworkManager.Singleton.client.Send(message, true);
        UIMenu.Singleton.ChatRoomUI();
        gamemng.instance.idOldChat = id;
    }
    [MessageHandler((ushort)ServerToClient.GetChatMessageInMenu)]
    private static void ReceiveGetMessageChatInMenu(Message message)
    {
        UIMenu.Singleton.MessageChatInMenu(message.GetString());
    }

    [SerializeField] private TMP_InputField messageInput;
    [SerializeField] private GameObject YourMs;
    [SerializeField] private GameObject yourname;
    [SerializeField] private Transform contentMessage;
    public void SendMessageBtn()
    {
        //UI
        Transform n = Instantiate(yourname, contentMessage).transform;
        n.GetComponent<TextMeshProUGUI>().text = gamemng.instance.playerinf.NamePlayer;
        n.SetAsFirstSibling();
        Transform messcontain = Instantiate(YourMs, contentMessage).transform;
        messcontain.SetAsFirstSibling();
        messcontain.GetChild(0).GetComponent<TextMeshProUGUI>().text = messageInput.text;
        //messcontain.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = new Time() now;
        //send to server
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.SendMessageMenu);
        message.AddInt(gamemng.instance.playerinf.Id);
        message.AddString(gamemng.instance.NameChat);
        message.AddString(gamemng.instance.playerinf.NamePlayer);
        message.AddString(messageInput.text);
        message.AddInt(gamemng.instance.idChatRoom);
        NetworkManager.Singleton.client.Send(message, true);
        messageInput.text = null;
    }

    //Receive message
    [MessageHandler((ushort)ServerToClient.SendMessageMenu)]
    private static void ReceiveMessage(Message message)
    {
        UIMenu.Singleton.ReceiveMessage(message.GetString(),message.GetString(), message.GetString(), message.GetInt());
    }
    //Friend
    [SerializeField] TextMeshProUGUI newAddFr;
    [SerializeField] private Transform GetAddfriend;
    [SerializeField] private GameObject AddFriendSuccess;
    public void FriendIcon()
    {
        if (!gamemng.instance.isgetaddfrie)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServer.GetAddFriendRequest);
            message.Add(gamemng.instance.playerinf.Id);
            NetworkManager.Singleton.client.Send(message, true);
            gamemng.instance.isgetaddfrie = true;
        }
        if (gamemng.instance.playerinf.NewAddFr != 0)
        {
            gamemng.instance.playerinf.NewAddFr = 0;
            newAddFr.text = "0";
        }
    }
    [MessageHandler((ushort)ServerToClient.GetAddFriendRequest)]
    private static void ReceiveResultGetAddFriendRequest(Message message)
    {
        UIMenu.Singleton.GetAddFriend(message.GetString());
    }
    public void NoAddFriend()
    {
        Transform button = EventSystem.current.currentSelectedGameObject.transform.parent;
        int id = int.Parse(button.name);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ReplyAddFriendRequest);
        message.AddInt(id);
        message.AddInt(0);
        NetworkManager.Singleton.client.Send(message, true);
        button.gameObject.SetActive(false);
    }
    public void AcceptAddFriend()
    {
        Transform button = EventSystem.current.currentSelectedGameObject.transform.parent;
        int id = int.Parse(button.name);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ReplyAddFriendRequest);
        message.AddInt(id);
        message.AddInt(1);
        NetworkManager.Singleton.client.Send(message, true);
        AddFriendSuccess.SetActive(true);
        button.gameObject.SetActive(false);
    }
    // AddFriend
    public void AddFriendRequestBtn(TMP_InputField Id)
    {
        int id = int.Parse(Id.text);
        Id.text = "";
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.AddFriendRequest);
        message.AddInt(gamemng.instance.playerinf.Id);
        message.AddString(gamemng.instance.playerinf.NamePlayer);
        message.AddInt(id);
        NetworkManager.Singleton.client.Send(message, true);
    }
    [MessageHandler((ushort)ServerToClient.AddFriendRequest)]
    private static void receiveAddFriendRequest(Message message)
    {
        UIMenu.Singleton.AddFriendREquest(message.GetInt(),message.GetString(),message.GetBool());
    }   
    public void successAddfriend()
    {
        if (GetAddfriend.childCount == 0) GetAddfriend.gameObject.SetActive(false);
    }


    //Join Room
    [SerializeField] private GameObject fullPlayer;
    public void JoinRoomBtn(TMP_InputField inputJoinRoom)
    {
        UIMenu.Singleton.Loading.SetActive(true);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.JoinRoom);
        message.Add(int.Parse(inputJoinRoom.text));
       // message.Add(gamemng.instance.playerinf.Id);
        NetworkManager.Singleton.client.Send(message);
        inputJoinRoom.text = "";
    }
    [MessageHandler((ushort)ServerToClient.JoinRoom)]
    private static void receiveJoinRoomResult(Message message)
    {
        UIMenu.Singleton.JoinRoomResult(message.GetString(), message.GetInt());
    }

    //Playergame now
    public void PlayegamenowBtn()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.PlayGameNow);
        NetworkManager.Singleton.client.Send(message, true);
    }
    [MessageHandler((ushort)ServerToClient.PlayGameNow)]
    private static void ReceivePlayGameNow(Message message)
    {
        UIMenu.Singleton.PlayNowResult(message.GetString(), message.GetInt());
    }

}
