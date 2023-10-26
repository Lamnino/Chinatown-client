using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

    public class ExchangeImform 
    {

    private byte id;
    private ushort idsent;
    private ushort idreceive;
    private List<byte> groundChose;
    private List<byte> groudGet;
    private List<byte> cardChose;
    private List<byte> cardGet;
    private int money;
    private int moneyGet;


    public byte Id
    {
        get { return id; }
         set { id = value;  }
    }
    public ushort IdSent
    {
        get { return idsent; }
        set { idsent = value; }
    }
    public ushort IdReceive
    {
        get { return idreceive; }
         set { idreceive = value; }
    }
    public List<byte> GrousdChose
    {
        get { return groundChose; }
         set { groundChose = value; }
    }
    public List<byte> GrousdGet
    {
        get { return groudGet; }
         set { groudGet = value;  }
    }
    public List<byte> CardChose
    {
        get { return cardChose; }
         set { cardChose = value; }
    }
    public List<byte> CardGet
    {
        get { return cardGet; }
         set { cardGet = value; }
    }
    public int Money
    {
        get { return money; }
         set { money = value; }
    }
    public int MoneyGet
    {
        get { return moneyGet; }
         set { moneyGet = value; }
    }
}

