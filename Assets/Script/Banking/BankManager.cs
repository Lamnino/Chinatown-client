using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

public class BankManager : MonoBehaviour
{
    [SerializeField] Transform contentExchangeItem;
    [SerializeField] Transform contentExchangeItemGet;
    public void DoneBtn()
    {
        for (int i=0; i < contentExchangeItem.childCount - 1; i++)
        {
            contentExchangeItem.GetChild(i).GetComponent<DropDownItemExchange>().destroy();
        }
        for (int i = 0; i < contentExchangeItemGet.childCount - 1; i++)
        {
            contentExchangeItemGet.GetChild(i).GetComponent<DropDownItemExchange>().destroy(); 
        }
    }
    public void commitbutton()
    {
        MainUI.instance.Loading.SetActive(true);
        Message message = Message.Create(MessageSendMode.Reliable,(ushort)ClientToServer.commitechange);
        message.AddInt(gamemng.instance.room.Idroom);
        message.AddUShort(gamemng.instance.idWhoYouExChangeItem);
        message.AddBytes(DropDownItemExchange.Groundchose.ToArray());
        message.AddBytes(DropDownItemExchange.GroundchoseGet.ToArray());
        message.AddBytes(DropDownItemExchange.cardChose.ToArray());
        message.AddBytes(DropDownItemExchange.cardChoseGet.ToArray());
        message.AddInt(DropDownItemExchange.money);
        message.AddInt(DropDownItemExchange.moneyget);
        NetworkManager.Singleton.client.Send(message, true);
    }
    [MessageHandler((ushort)ServerToClient.commitechange)]
    private static void Receivecommitechange(Message message)
    {
        BankUI.instance.commitchangeitem(message.GetString());
    }
    public static GameObject ExchangerequestBtnToDelete;
    public void ReplyExchangeItemRequestbtn(bool result)
    {
        MainUI.instance.Loading.SetActive(true);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ReplyExchangeItemRequest);
        message.AddInt(gamemng.instance.room.Idroom);
        message.AddByte(gamemng.instance.IdRequestExchangeItem);
        message.AddBool(result);
        NetworkManager.Singleton.client.Send(message, true);
        
    }
    [MessageHandler((ushort)ServerToClient.ReplyExchangeItemRequest)]
    private static void ReceiveExchangeitemRequest(Message message)
    {
        MainUI.instance.ReplyReplyExchangeItemRequest(message.GetByte(), message.GetBool());
    }
    [SerializeField] private TMP_InputField amountLoan;
    [SerializeField] private TMP_Dropdown Period;
    [SerializeField] private TMP_Dropdown SourceLoan;
    [SerializeField] private TMP_InputField rateLoan;
    public void SourceLoanChangeValue(int index)
    {
        if (index == 0)
        {
            rateLoan.text = "9,87";
            rateLoan.interactable = false;
        }
        else
        {
            rateLoan.text = "4,56";
            rateLoan.interactable = true;
        }
    }
    public static bool ischangeRateLoan = false;
    public void commitLoanBtn()
    {
        int MonyeLoan = 0;
        if (int.TryParse(amountLoan.text, out MonyeLoan))
        {
            float rate = 0;
            if (float.TryParse(rateLoan.text, out rate))
            {
                MainUI.instance.Loading.SetActive(true);
                ushort source = 0;
                if (SourceLoan.value != 0)
                {
                    source = gamemng.instance.room.Players.Find(item => RoomManager.index(item.id) == SourceLoan.value-1).id;
                }
                Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.commitLoan);
                message.AddInt(gamemng.instance.room.Idroom);
                message.AddInt(MonyeLoan);
                message.AddByte(byte.Parse(Period.options[Period.value].text));
                message.AddUShort(source);
                message.AddFloat(rate);
                NetworkManager.Singleton.client.Send(message, true);
            }
            else
            {
                MainUI.instance.Displayerannouce("please type rate you want", true);
            }
        }
        else
        {
            MainUI.instance.Displayerannouce("please type amount you want to loan", true);
        }
    }
    [MessageHandler((ushort)ServerToClient.commitLoan)]
    private static void ReceivecommitLoan(Message message)
    {
        BankUI.instance.commitLoan(message); 
    }
    [SerializeField] TMP_InputField rateloanNew;
    public void ReplyLoan(bool result)
    {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ReplyLoan);
        message.AddInt(gamemng.instance.room.Idroom);
        message.AddByte(gamemng.instance.idRequestLoan);
        MainUI.instance.Loading.SetActive(true);
        if (!result)
        {
            message.AddBool(false);
            NetworkManager.Singleton.client.Send(message, true);
        }
        else
        {
                message.AddBool(true);
            if (!ischangeRateLoan)
            {
                message.AddBool(false);
                NetworkManager.Singleton.client.Send(message, true);
            }
            else
            {
                ischangeRateLoan = false;
                message.AddBool(true);
                float newrate = 0;
                if (float.TryParse(rateloanNew.text,out newrate))
                {
                    message.AddDouble(newrate);
                    NetworkManager.Singleton.client.Send(message, true);
                    gamemng.instance.room.loans.Find(item => item.Id == gamemng.instance.idRequestLoan).Rate = newrate;
                }   
                else
                {
                    MainUI.instance.Displayerannouce("please type interest rate you want", true);
                } 
            }
        }
    }
    [MessageHandler((ushort)ServerToClient.ReplyLoan)]
    private static void ReceiveReplyLoan(Message message)
    {
        BankUI.instance.ReplyLoan(message);
    }
    public static byte idLoanToReLoan = 0;
    public void ReLoanBtnClick(bool result)
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.ReLoan);
        message.AddInt(gamemng.instance.room.Idroom);
        message.AddByte(idLoanToReLoan);
        message.AddBool(result);
        NetworkManager.Singleton.client.Send(message, true);
        MainUI.instance.Loading.SetActive(true);
    }
    [MessageHandler((ushort)ServerToClient.ReLoan)]
    private static void ReceiveReLoan(Message message)
    {
        BankUI.instance.Reloan(message);
    }
    public void Depositebtn()
    {
        int amountDeposite = 0;
        if (int.TryParse(BankUI.instance.DepositeAmount.text, out amountDeposite))
        {
            Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.Deposite);
            message.AddInt(gamemng.instance.room.Idroom);
            message.AddInt(amountDeposite);
            message.AddByte(byte.Parse(BankUI.instance.PeriodDeposite.options[BankUI.instance.PeriodDeposite.value].text));
            NetworkManager.Singleton.client.Send(message, true);
            MainUI.instance.Loading.SetActive(true);
        }
        else
        {
            MainUI.instance.Displayerannouce("please type amount you want to deposite", true);
        }
    }
    [MessageHandler((ushort)ServerToClient.Deposite)]
    private static void ReceiveDeposite(Message message)
    {
        BankUI.instance.Deposite(message);
    }
    public void PayBtnClick()
    {

    }
}
