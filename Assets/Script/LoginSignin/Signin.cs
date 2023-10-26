using System.Collections;
using System.Collections.Generic;
using Riptide;
using UnityEngine;

public class Signin : MonoBehaviour
{
    public static string  result;
    [MessageHandler((ushort)ServerToClient.Signin)]
    private static void ReceiveResult(Message message)
    {
        result=message.GetString();
    }
}
