using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;
public class Login : MonoBehaviour
{
 
    public static string resultt;
    public void SendCheck(string EmailLogin, string PassLogin)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.Login);
        message.Add(EmailLogin);
        message.Add(PassLogin);
        NetworkManager.Singleton.client.Send(message,true);
    }
    [MessageHandler((ushort)ServerToClient.Login)]
    public static void Receive(Message message)
    {
        LoginUI.instance.loginUi(message.GetString(), message.GetUShort());
    }
    
}

