using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;

public class ChoseColor : MonoBehaviour
{
    public static ChoseColor instance;
    [SerializeField] private Button sendcolorBtn;
    byte Color;
    private void Start()
    {
        instance = this;
    }

    public void colorClick(int color)
    {
       Color = (byte)color;
        byte c1 = 0, c2 = 0, c3 = 0;
        switch (Color)
        {
            case 1:
                c1 = 200; c2 = 0; c3 = 0 ;
                break;
            case 2:
                c1 = 200; c2 = 100; c3 =  0;
                break;
            case 3:
                c1 = 200; c2 = 200; c3 =  200;
                break;
            case 4:
                c1 = 50; c2 = 200; c3 =  0;
                break;
            case 5:
                c1 = 0; c2 = 50; c3 =  200;
                break;
        }
        sendcolorBtn.GetComponent<Image>().color = new Color32(c1,c2,c3,255);
    }
    // Update is called once per frame
    public void CommitColorBtn()
    {
        gamemng.instance.room.You.setColor(Color);
    }
}
