using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public class UIMenu : MonoBehaviour
{
    public static UIMenu singleton;
    public static UIMenu Singleton
    {
        get => singleton;
        set
        {
            if (singleton == null) 
                singleton = value; 
            else Destroy(value);
        }
    }
    private void Awake()
    {
        singleton = this;
    }

    [SerializeField] private TextMeshProUGUI NamePlayer;
    [SerializeField] private TextMeshProUGUI Point;
    [SerializeField] private TextMeshProUGUI newAddFrRequest;
    [SerializeField] private TextMeshProUGUI newMessage;
    [SerializeField] private TextMeshProUGUI newNotification;
    private void Start()
    {
        NamePlayer.text = gamemng.instance.playerinf.NamePlayer;
        Point.text = gamemng.instance.playerinf.Point.ToString();
        newAddFrRequest.text = gamemng.instance.playerinf.NewAddFr.ToString();
        newMessage.text = gamemng.instance.playerinf.NewMessage.ToString();
        newNotification.text = gamemng.instance.playerinf.NewNotification.ToString();
    }

    //avatar
    [SerializeField] private Image Avatarobj;
    byte[] data = new byte[0];
    public void Avatar(byte[] bytes, bool isfinish)
    {
        data = data.Concat(bytes).ToArray();
        
        if (isfinish)
        {
        Debug.Log("Get Avarta success");
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(data);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Avatarobj.sprite = sprite;
        }
    }
    //Notification
    [SerializeField] private Transform contentNotification;
    [SerializeField] private GameObject NotificationPrefab;
        public void NotificationUI(string jsonarray)
    {
        List<Notification> notis = JsonConvert.DeserializeObject<List<Notification>>(jsonarray);
        foreach (var noti in notis)
        {
            Transform notification = Instantiate(NotificationPrefab, contentNotification).transform;
            notification.GetChild(0).GetComponent<TextMeshProUGUI>().text = noti.title;
            notification.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = noti.content;
        }
    }
    //List conversation
    List<Conversation> conversations = new List<Conversation>();
    [SerializeField] private GameObject conversation;
    [SerializeField] private Transform conntentlisconversation;
    [SerializeField] private GameObject ListConversationobj;
    [SerializeField] private GameObject MessageChat;
    float contentListConversationSize = 0;
    public void ListConversation(string json)
    {
        conversations = JsonConvert.DeserializeObject<List<Conversation>>(json);
        foreach (var con in conversations)
        {
            DisplaylistConversation(con);
        }
        //conntentlisconversation.localScale = new Vector3(0, contentListConversationSize, 0);
        newMessage.text = "0";
    }
    private void DisplaylistConversation(Conversation con)
    {
            Transform conn = Instantiate(conversation, conntentlisconversation).transform;
            conn.name = con.id;
            Transform NamePlace = conn.GetChild(0);
            // Set name and Size
            NamePlace.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = con.nameRoom;
            TextMeshProUGUI NameTextField = NamePlace.GetChild(0).GetComponent<TextMeshProUGUI>();
            NameTextField.rectTransform.sizeDelta = new Vector2(NameTextField.preferredWidth + 2, 0);
            //Set greenTick
            if (con.state == "0")
                NamePlace.GetChild(1).gameObject.SetActive(false);
            else
                NamePlace.GetChild(1).gameObject.SetActive(true);
            // Last messahe
            conn.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = con.lastMessage;
            conn.GetChild(2).GetComponent<TextMeshProUGUI>().text = con.id;
            Button btn = conn.GetComponent<Button>();
            btn.onClick.AddListener(new UnityEngine.Events.UnityAction(MenuManager.Singleton.GetMessageChat));
            btn.onClick.AddListener(new UnityEngine.Events.UnityAction(hideListConversation));
            //contentListConversationSize += conn.GetComponent<Renderer>().bounds.size.y;


    }
    private void hideListConversation()
    {
        ListConversationobj.SetActive(false);
        MessageChat.SetActive(true);
    }

    // MessageChatInMenu
    [SerializeField] private Transform ContainMessageChat;
    [SerializeField] private TextMeshProUGUI FriendName;
    [SerializeField] private TextMeshProUGUI YourName;
    [SerializeField] private Transform yourText;
    [SerializeField] private Transform FriendText;
    List<Messagee> messages = new List<Messagee>();
    public void MessageChatInMenu(string json)
    {
        for (int  i=0; i< ContainMessageChat.childCount; i++)
        {
            Destroy(ContainMessageChat.GetChild(i).gameObject);
        }
        messages = JsonConvert.DeserializeObject<List<Messagee>>(json);
        foreach (var mess in messages)
        {
            TextMeshProUGUI Name;
            Transform Messageee;
            if (mess.Id == gamemng.instance.playerinf.Id)
            {
                Messageee = Instantiate(yourText, ContainMessageChat);
                Name = Instantiate(YourName, ContainMessageChat);
            }
            else
            {
                Messageee = Instantiate(FriendText, ContainMessageChat);
                Name = Instantiate(FriendName, ContainMessageChat);
            }
            Name.text = mess.NamePlayer;
            Messageee.GetChild(0).GetComponent<TextMeshProUGUI>().text = mess.message;
            Messageee.GetChild(1).GetComponent<TextMeshProUGUI>().text = mess.time;
        }
    }
    [SerializeField] private TextMeshProUGUI NameChatRoom;
    [SerializeField] private GameObject StateChatRoom;
    public void ChatRoomUI()
    {
        NameChatRoom.text = gamemng.instance.NameChat;
        StateChatRoom.SetActive(gamemng.instance.stateChat);
    }

    //Receive message
    [SerializeField] private GameObject messageobj;
    public void ReceiveMessage(string namechat , string message, string name, int idRoom)
    {
        Debug.Log("here");
        if (!messageobj.activeSelf)
        {
            gamemng.instance.newMessage++;
            newMessage.text = gamemng.instance.newMessage.ToString();
        } else
        {
            if (ListConversationobj.activeSelf)
            {
                changeIndexListConverstion(idRoom, message);
            }
            else
            {
                Transform n = Instantiate(FriendName, ContainMessageChat).transform;
                n.GetComponent<TextMeshProUGUI>().text = name;
                n.SetAsFirstSibling();
                Transform m = Instantiate(FriendText, ContainMessageChat).transform;
                m.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
                m.SetAsFirstSibling();
            }
        }
    }
    private void changeIndexListConverstion(int idroom, string message)
    {
        int i = 0;
        foreach (var con in conversations)
        {
            if (con.id == idroom.ToString())
            {
                Debug.Log(i);
                con.lastMessage = message;
                //conversations.Insert(0,con);
                conntentlisconversation.GetChild(i).SetAsFirstSibling();
                conntentlisconversation.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
                break;  
            }
            i++;
        }
    }

    //Friend
    [SerializeField] private GameObject FriendRequestInform;
    [SerializeField] private GameObject FriendRequestBG;
    [SerializeField] private Transform contentGetFriendResquest;
    List<FriendRequest> FriendRequests; 

    public void GetAddFriend(string result)
    {
        if (result == "no")
        {
            FriendRequestBG.SetActive(false);
        }
        else
        {
            FriendRequestBG.SetActive(true);
            FriendRequests = JsonConvert.DeserializeObject<List<FriendRequest>>(result);
            foreach ( var frrequest in FriendRequests)
            {
                Transform addfr = Instantiate(FriendRequestInform, contentGetFriendResquest).transform;
                addfr.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = frrequest.NamePlayer;
                addfr.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = frrequest.message;
                addfr.name = frrequest.id;
                addfr.GetChild(1).GetComponent<Button>().onClick.AddListener(MenuManager.Singleton.AcceptAddFriend);
                addfr.GetChild(2).GetComponent<Button>().onClick.AddListener(MenuManager.Singleton.NoAddFriend);
            }
        }
    }
    //AddfriendRequest
    [SerializeField] private GameObject WereFreind;
    [SerializeField] private GameObject Success;
    [SerializeField] private GameObject None;
    [SerializeField] private GameObject Main;
    public void AddFriendREquest(int id,string result, bool IsResult)
    {
        Debug.Log(result);
        if (IsResult)
        {
            if (result == "Were Friend")
                WereFreind.SetActive(true);
            else
                if (result == "exist" || result == "ok")
                Success.SetActive(true);
            else
                if (result == "none")             
                None.SetActive(true);
            else Debug.Log(result);
        }
        else
        {
            if (Main.activeSelf)
            {
                FriendRequestBG.SetActive(true);
                Transform addfr = Instantiate(FriendRequestInform, contentGetFriendResquest).transform;
                addfr.SetAsFirstSibling();
                addfr.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = result;
                addfr.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "add friend";
                addfr.name = id.ToString();
                addfr.GetChild(1).GetComponent<Button>().onClick.AddListener(MenuManager.Singleton.AcceptAddFriend);
                addfr.GetChild(2).GetComponent<Button>().onClick.AddListener(MenuManager.Singleton.NoAddFriend);
            }
            else
            {
                gamemng.instance.playerinf.NewAddFr += 1;
                newAddFrRequest.text = gamemng.instance.playerinf.NewAddFr.ToString();
                gamemng.instance.isgetaddfrie = false;
                for (int i = 0; i<contentGetFriendResquest.childCount; i++)
                {
                    contentGetFriendResquest.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    // Join room
    [SerializeField] public GameObject Loading;
    [SerializeField] private GameObject fullMember;
    [SerializeField] private GameObject RoomIsStart;
    [SerializeField] private GameObject SuccessJoinRoom;
    public void JoinRoomResult(string result,int idrom)
    {
        Loading.SetActive(false);
        if (result == "FullMember")
        {
            fullMember.SetActive(true);
        }
        else 
        if (result == "NewRoom")
        {
            SuccessJoinRoom.SetActive(true);
                gamemng.instance.room =  new RoomManager(idrom);
        }
        else
        {
            SuccessJoinRoom.SetActive(true);
            gamemng.instance.room = new RoomManager(result, idrom);
        }
    }
    //join game now
    public void PlayNowResult(string inform, int idroom)
    {
        if (inform == "NewRoom")
        {
            gamemng.instance.room = new RoomManager(idroom);
        }
        else
        {
            gamemng.instance.room = new RoomManager(inform, idroom);
        }
    }

    public void JoinGameSuccessBtn()
    {
        SceneManager.LoadScene("gameScene");
    }
}
