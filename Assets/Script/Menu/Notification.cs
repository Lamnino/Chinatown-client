using System;
using UnityEngine;

[System.Serializable]
public class Notification
{
    private string Title;
    private string Content;
    private bool State;
    public string title
    {
        get { return Title; }
        set { Title = value; }
    }
    public string content
    {
        get { return Content; }
        set { Content = value; }
    }
    public bool state
    {
        get { return State; }
        set { State = value; }
    }
}