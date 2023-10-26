using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownItemExchange : MonoBehaviour
{
    TMP_Dropdown option;
    TMP_InputField Money;
    TMP_Dropdown numberOption;
    RoomManager room;
    public byte eog = 0;
    private byte previousground =100;
    private byte previouscard=100;
    private byte previousgroundg =100;
    private byte previouscardg=100;
    private int previousmoney = 0;
    private int previousmoneyg = 0;
    private byte previousOption = 0;
    private byte previousOptiong = 0;
    public static List<byte> Groundchose = new List<byte>();
    public static List<byte> cardChose = new List<byte>();
    public static List<byte> GroundchoseGet = new List<byte>();
    public static List<byte> cardChoseGet = new List<byte>();
    public static int money = 0;
    public static int moneyget = 0;

    private void Start()
    {
        room = gamemng.instance.room;
        option = transform.GetChild(0).GetComponent<TMP_Dropdown>();
        Money = transform.GetChild(1).GetComponent<TMP_InputField>();
        numberOption = transform.GetChild(2).GetComponent<TMP_Dropdown>();
        optionGround(true);
        
    }
    public void optionchangevalue(int index)
    {
        if (previousOption == 1)
        {
            if (eog == 0)
            {
                if (room.You.Card()[previouscard] - cardChose[previouscard] == 0)
                    UpdateCardOption(previouscard, true);
                cardChose[previouscard]--;
            }
            else
            {
                if (room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem).Card()[previouscardg] - cardChoseGet[previouscardg] == 0)
                    UpdateCardOption(previouscardg, true);
                cardChoseGet[previouscardg]--;
            }
        }
        else if (previousOption == 0)
        {
            if (eog == 0)
            {
                Groundchose.Remove(previousground);
                UpdateGroundOption(previousground, true);
            }
            else
            {
                GroundchoseGet.Remove(previousgroundg);
                UpdateGroundOption(previousgroundg, true);
            }
        }
        else
        {
            money -= previousmoney;
        }
        switch (index)
        {

            case 0:
                previousOption = 0;
                optionGround(true);
                break;
            case 1:
                previousOption = 1;
                optionCard(true);
                break;
            case 2:
                Money.gameObject.SetActive(true);
                numberOption.gameObject.SetActive(false);
                previousOption = 2;
                break;
        }
    }
    private void optionGround(bool start)
    {
        Money.gameObject.SetActive(false);
        numberOption.gameObject.SetActive(true);
        numberOption.options.Clear();
        PlayerInRomm who;
        if (eog == 0)
        {
            previousOption = 0;
            if (!start)
            {
                if (Groundchose.Contains(previousground)) Groundchose.Remove(previousground);
            }
            who  = room.You;
            List<TMP_Dropdown.OptionData> optiondata = new List<TMP_Dropdown.OptionData>();
            foreach (var optiongr in who.Ground())
            {
                bool check = true;
                foreach (var gc in Groundchose)
                {
                    if (optiongr == gc)
                    {
                        check = false; break;
                    }
                }
                if (check)
                    optiondata.Add(new TMP_Dropdown.OptionData(optiongr.ToString()));
            }
            previousground = byte.Parse(optiondata[0].text);
            Groundchose.Add(previousground);
            UpdateGroundOption(previousground, false);
            numberOption.AddOptions(optiondata);
        }
        else
        {
            previousOptiong = 0;
            if (!start)
            {
                if (GroundchoseGet.Contains(previousgroundg)) GroundchoseGet.Remove(previousgroundg);
            }
            who = room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem);
            List<TMP_Dropdown.OptionData> optiondata = new List<TMP_Dropdown.OptionData>();
            foreach (var optiongr in who.Ground())
            {
                bool check = true;
                foreach (var gc in GroundchoseGet)
                {
                    if (optiongr == gc)
                    {
                        check = false; break;
                    }
                }
                if (check)
                    optiondata.Add(new TMP_Dropdown.OptionData(optiongr.ToString()));
            }
            previousgroundg = byte.Parse(optiondata[0].text);
            GroundchoseGet.Add(previousgroundg);
            numberOption.AddOptions(optiondata);
            UpdateGroundOption(previousgroundg, false);
        }
    }
    private void optionCard(bool start)
    {
        Money.gameObject.SetActive(false);
        numberOption.gameObject.SetActive(true);
        numberOption.options.Clear();
        previousOption = 1;
        PlayerInRomm who;
        if (eog == 0)
        {
            who = room.You;
            if (!start)
            {
                cardChose[previouscard]--;
            }
            List<TMP_Dropdown.OptionData> optiondata = new List<TMP_Dropdown.OptionData>();
            for (byte i = 0; i < 12; i++)
            {
                if (who.Card()[i] != 0 && who.Card()[i] - cardChose[i] > 0)
                {
                    optiondata.Add(new TMP_Dropdown.OptionData(numtocard(i)));
                }
            }
            numberOption.AddOptions(optiondata);
            previouscard = cardtonumber(optiondata[0].text);
            cardChose[previouscard]++;
            if  (who.Card()[previouscard] - cardChose[previouscard] == 0)
            {
                UpdateCardOption(previouscard,false);
            }
        }
        else
        {
            who = room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem);
            if (!start) cardChoseGet[previouscardg]--;
            List<TMP_Dropdown.OptionData> optiondata = new List<TMP_Dropdown.OptionData>();
            for (byte i = 0; i < 12; i++)
            {
                if (who.Card()[i] != 0 && who.Card()[i] - cardChoseGet[i] > 0)
                    optiondata.Add(new TMP_Dropdown.OptionData(numtocard(i)));
            }
            numberOption.AddOptions(optiondata);
            previouscardg = cardtonumber(optiondata[0].text);
            cardChoseGet[previouscardg]++;
            if (who.Card()[previouscardg] - cardChoseGet[previouscardg] == 0)
            {
                UpdateCardOption(previouscardg, false);
            }
        }
    }
    public void numberChangevalue(int index)
    {
        if(eog == 0)
        {
            if (option.value == 0)
            {
                Groundchose.Remove(previousground);
                UpdateGroundOption(previousground, true);
                previousground = (byte.Parse(numberOption.options[index].text));
                Groundchose.Add(previousground);
                UpdateGroundOption(previousground, false);
            }
            else
                  if (option.value == 1)
            {
                if (room.You.Card()[previouscard] - cardChose[previouscard] == 0)
                    UpdateCardOption(previouscard, true);
                cardChose[previouscard]--;
                previouscard = cardtonumber(numberOption.options[index].text);
                cardChose[previouscard]++;
                if (room.You.Card()[previouscard] - cardChose[previouscard] == 0)
                {
                    UpdateCardOption(previouscard, false);
                }
            }
        }
        else
        {
            if (option.value == 0)
            {
                GroundchoseGet.Remove(previousgroundg);
                UpdateGroundOption(previousgroundg, true);
                previousgroundg = (byte.Parse(numberOption.options[index].text));
                GroundchoseGet.Add(previousgroundg);
                UpdateGroundOption(previousgroundg, false);
            }
            else
                if (option.value == 1)
            {
                if (room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem).Card()[previouscardg] - cardChoseGet[previouscardg] == 0)
                    UpdateCardOption(previouscardg, true);
                cardChoseGet[previouscardg]--;
                previouscardg = cardtonumber(numberOption.options[index].text);
                cardChoseGet[previouscardg]++;
                if (room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem).Card()[previouscardg] - cardChoseGet[previouscardg] == 0)
                {
                    UpdateCardOption(previouscardg, false);
                }
            }
        }
    }
    public void moneychange(string newmoney) 
    {
        int newMoney;
        if (!string.IsNullOrEmpty(newmoney) && int.TryParse(newmoney, out newMoney))
        {
            if (eog == 0)
            {
                money -= previousmoney;
                money += newMoney;
                previousmoney = newMoney;
            }
            else
            {
                moneyget -= previousmoneyg;
                moneyget += newMoney;
                previousmoneyg = newMoney;
            }
        }
        else
        {
            MainUI.instance.Displayerannouce("please enter amount you want to exchange", true);
            Money.ActivateInputField();
        }
    }
    public void destroy()
    {
        if (option.value == 1)
        {
            if (eog == 0)
            {
                if (room.You.Card()[previouscard] - cardChose[previouscard] == 0)
                    UpdateCardOption(previouscard, true);
                cardChose[previouscard]--;
            }
            else
            {
                if (room.Players.Find(item => item.id == gamemng.instance.idWhoYouExChangeItem).Card()[previouscardg] - cardChoseGet[previouscardg] == 0)
                    UpdateCardOption(previouscardg, true);
                cardChoseGet[previouscardg]--;
            }
        }
        else if (option.value == 0)
        {
            if (eog == 0)
            {
                Groundchose.Remove(previousground);
                UpdateGroundOption(previousground, true);
            }
            else
            {
                GroundchoseGet.Remove(previousgroundg);
                UpdateGroundOption(previousgroundg, true);
            }
        }
        else
        {
            if (eog == 0)
                money -= int.Parse(Money.text);
            else
                moneyget -= int.Parse(Money.text);
        }
        Destroy(gameObject);
    }
    public void AddGroundOption(byte ground)
    {
        string optionAdd = ground.ToString();
        TMP_Dropdown.OptionData newoption = new TMP_Dropdown.OptionData(optionAdd);
        numberOption.options.Add(newoption);
        numberOption.RefreshShownValue();
    }
    public void AddCardOption(byte card)
    {
        string optionAdd = numtocard(card);
        TMP_Dropdown.OptionData newoption = new TMP_Dropdown.OptionData(optionAdd);
        numberOption.options.Add(newoption);
        numberOption.RefreshShownValue();
    }
    public void removeOptionCard(byte card)
    {
        List<TMP_Dropdown.OptionData> options = numberOption.options;
        string optionMove = numtocard(card);
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].text == optionMove)
            {
                options.RemoveAt(i);
                if (i < numberOption.value) numberOption.value--;
                numberOption.RefreshShownValue();
                return;
            }
        }
    }
    public void UpdateCardOption(byte card,bool add)
    {
        Transform parent = transform.parent;
        DropDownItemExchange drscript;
        string namecard = numtocard(card);
        for (int i = 0; i < parent.childCount - 1; i++)
        {
            drscript = parent.GetChild(i).GetComponent<DropDownItemExchange>();
            if (drscript.option.value == 1 &&  drscript != this)
            {
                if (add)
                {
                    drscript.AddCardOption(card);
                }
                else
                {
                    if ( cardtonumber(drscript.numberOption.options[drscript.numberOption.value].text) != card)
                    {
                        drscript.removeOptionCard(card);
                    }
                }
            }
           
        }
    }
    public void RemoveOptionChoseGround(byte ground)
    {
        string optionMove = ground.ToString();
        List<TMP_Dropdown.OptionData> options = numberOption.options;
        for (int i=0; i < options.Count; i++)
        {
            if (options[i].text == optionMove)
            {
                options.RemoveAt(i);
                if (numberOption.value > i) numberOption.value--;
                numberOption.RefreshShownValue();
                return;
            }
        }
    }
    public void UpdateGroundOption(byte optionMove,bool add)
    {
        Transform parent = transform.parent;
        DropDownItemExchange drscript;
        for (int i=0; i < parent.childCount-1; i++)
        {
            drscript = parent.GetChild(i).GetComponent<DropDownItemExchange>();
            if (drscript != this && drscript.option.value == 0)
            {
                if (add)
                { 
                    drscript.AddGroundOption(optionMove);
                }
                else
                {
                    drscript.RemoveOptionChoseGround(optionMove);
                }
            }
        }
    }
    public string numtocard(byte c)
    {
        switch (c)
        {
            case 0:
                return "photo";
            case 1:
                return "teahouse";
            case 2:
                return "seafood";
            case 3:
                return "jewellery";
            case 4:
                return "tropical";
            case 5:
                return "florist";
            case 6:
                return "takeout";
            case 7:
                return "laundry";
            case 8:
                return "dimsum";
            case 9:
                return "antique";
            case 10:
                return "factory";
            case 11:
                return "hotel";
            default:
                return null;
        }
    }
    public byte cardtonumber(string ca)
    {
        switch (ca)
        {
            case "photo":
                return 0;
            case "teahouse":
                return 1;
            case "seafood":
                return 2;
            case "jewellery":
                return 3;
            case "tropical":
                return 4;
            case "florist":
                return 5;
            case "takeout":
                return 6;
            case "laundry":
                return 7;
            case "dimsum":
                return 8;
            case "antique":
                return 9;
            case "factory":
                return 10;
            case "hotel":
                return 11;
            default:
                return 13;
        }
    }
}
