using UnityEngine;
using System;
public class PlayerInf
{
    public int Id;
    public string NamePlayer;
    public int Point;
    public int NewAddFr;
    public int NewMessage;
    public int NewNotification;
    public PlayerInf(int id, string name, int point)
    {
        Id = id; NamePlayer = name; Point = point;
    }
}