using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Ground : MonoBehaviour
{
    bool ischose = false;
    public bool owner = false;
    bool allowClick = true;
    public byte numGround = 0;
    public static List<byte> listGround = new List<byte>();
    public static List<byte> GroundChose = new List<byte>();
    public static bool enough = false;
    Image thisimager;
    public static string Groundchose()
    {
        string result = JsonConvert.SerializeObject(GroundChose);
        GroundChose.Clear();
        return result;
    }
    public static string GroudLost()
    {
        string result = JsonConvert.SerializeObject(listGround);
        listGround.Clear();
        return result;
    }
    
    private void Start()
    {
        thisimager = gameObject.GetComponent<Image>();
    }
    public void hideGround()
    {
        byte color = 150;
        thisimager.color = new Color32(color, color, color,255);
        allowClick = false;
    }
    public void displayGround()
    {
        byte color = 255;
        thisimager.color = new Color32(color, color, color,255);
        allowClick = true;  
    }
    public static Color32 GetColor(byte color,byte lightness)
    {
        byte c1 = 0, c2 = 0, c3 = 0;
        switch (color)
        {
            case 1:
                c1 = 200; c2 = 0; c3 = 0;
                break;
            case 2:
                c1 = 200; c2 = 100; c3 = 0;
                break;
            case 3:
                c1 = 200; c2 = 200; c3 = 200;
                break;
            case 4:
                c1 = 50; c2 = 200; c3 = 0;
                break;
            case 5:
                c1 = 0; c2 = 50; c3 = 200;
                break;
        }
        return new Color32(c1, c2, c3, lightness);
    }
    public void SetColor(byte color)
    {
        transform.GetChild(0).GetComponent<Image>().color = GetColor(color,100);
    }

    public void groundClick()
    {
        switch (gamemng.instance.state)
        {
            case "none action":
                return;
            case "chose ground":
                if (allowClick && !owner)
                {
                    if (gamemng.instance.state == "chose ground")
                    {
                        if (!ischose)
                        {
                            if (!enough)
                            {
                                GroundChose.Add(numGround);
                                listGround.Remove(numGround);
                                ischose = true;
                                transform.GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(0).GetComponent<Image>().color = GetColor(gamemng.instance.room.You.Color(),100);
                                if (GroundChose.Count == gamemng.instance.numGroundIsChose)
                                {
                                    enough = true;
                                }
                            }
                        }
                        else
                        {
                            GroundChose.Remove(numGround);
                            listGround.Add(numGround);
                            if (enough) enough = false;
                                transform.GetChild(0).gameObject.SetActive(false);
                            ischose = false;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
    public static List<byte> Groundchoses()
    {
        return GroundChose;
    }
}
