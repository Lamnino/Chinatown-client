using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using Riptide;
public static class ButtonExtension
{
    public static void AddEventListener<t> ( this Button button, t pama, Action<t> Onclick)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick(pama);
        });
    }
    public static void AddEventListener<t,q>(this Button button, t pama1,q pama2, Action<t> Onclick1, Action<q> Onclick2)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick1(pama1);
            Onclick2(pama2);
        });
    }
}
public class BankUI : MonoBehaviour
{
    public static BankUI instance;
  
    RoomManager room;
    List<PlayerInRomm> member;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        room = gamemng.instance.room;
        BankUI.instance.UIWhenStartGame();
    }
    [SerializeField] private TextMeshProUGUI NameCard;
    [SerializeField] private TextMeshProUGUI NumberCard;
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI dattime;
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] private TextMeshProUGUI loan;
    [SerializeField] private TextMeshProUGUI deposite;
    string[] month = new string[12] {"January",  "February", "March", "April", "May",  "June","July", "August","September","Octobor","November","December"};
    public void setUIStart()
    {
        NameCard.text = gamemng.instance.playerinf.NamePlayer;
        NumberCard.text = $"190 373 739 {room.You.idserver}";
        Money.text = room.You.Cash() - room.You.Loan() + "$";
        dattime.text = $"{DateTime.Now.DayOfWeek} {DateTime.Now.Day} {month[DateTime.Now.Month-1]} {DateTime.Now.Year}";
        cash.text = room.You.Cash().ToString()+"$";
        loan.text = room.You.Loan().ToString() + "$";
        deposite.text = room.You.Deposite().ToString() + "$";
        ColorMenubtn(0);
    }
    [SerializeField] Transform contentmenubtn;
    public void ColorMenubtn(int index)
    {
        for (int i = 0; i< 4; i++)
        {
            Transform child = contentmenubtn.GetChild(i);
            if (i == index)
            {
                child.GetComponent<Image>().color = new Color32(21, 21, 21, 255); 
            }
            else
            {
                child.GetComponent<Image>().color = new Color32(87, 87, 87, 255);
            }
        }
    }
    //set UI when start game:
    [SerializeField] GameObject whoExchange;
    [SerializeField] Transform contentwhoexchange;
    [SerializeField] TMP_InputField AmountLoan;
    [SerializeField] private TMP_Dropdown periodLoan;
    [SerializeField] private TMP_Dropdown SourcecLoan;
    [SerializeField] private TMP_InputField Ratetext;
    public void UIWhenStartGame()
    {
        member = gamemng.instance.room.Players;
        // who change itemexchange option
        byte i = 1;
        foreach (var m in member)
        {
            if (m.id != gamemng.instance.room.You.id)
            {
                Transform wh = Instantiate(whoExchange, contentwhoexchange).transform;
                wh.GetChild(0).GetComponent<TextMeshProUGUI>().text = m.Name;
                wh.GetComponent<Image>().color = Ground.GetColor(i, 255);//m.Color()
                wh.GetComponent<Button>().AddEventListener(m.id, setIdExchange);
                i++;
            }
        }
        LoanoptionBtn();
        setUIStart();
    }
    public void LoanoptionBtn()
    {
         periodLoan.options.Clear();
        SourcecLoan.options.Clear();
        AmountLoan.text = "";
        for (int i = 6; i >= RoomManager.Year() ; i--)
        {
            TMP_Dropdown.OptionData new3 = new TMP_Dropdown.OptionData();
            new3.text = (6-i).ToString();
            periodLoan.options.Add(new3);
        }
        TMP_Dropdown.OptionData new1 = new TMP_Dropdown.OptionData();
        new1.text = "Bank";
            SourcecLoan.options.Add(new1);
        foreach (PlayerInRomm pl in gamemng.instance.room.Players)
        {
            TMP_Dropdown.OptionData new2 = new TMP_Dropdown.OptionData();
            if (pl != gamemng.instance.room.You)
            {
                new2.text = pl.Name;
                SourcecLoan.options.Add(new2);
            }
        }
            SourcecLoan.value = 0;
        Ratetext.text = "9,87";
    }
    public TMP_InputField DepositeAmount;
    public TMP_Dropdown PeriodDeposite;
    [SerializeField] TextMeshProUGUI RateDeposite;
    public void Depositeoptionbtn()
    {
        DepositeAmount.text = "";
        PeriodDeposite.options.Clear();
        int d = 1;
        for (int i = RoomManager.Year()+1; i < 6; i++)
        {
            TMP_Dropdown.OptionData new3 = new TMP_Dropdown.OptionData();
            new3.text = d.ToString();
            d++;
            PeriodDeposite.options.Add(new3);
            PeriodDeposite.value = 0;
        }
        switch (RoomManager.Year())
        {
            case 0:
                RateDeposite.text = "60%";
                break;
            case 1:
                RateDeposite.text = "50%";
                break;
            case 2:
                RateDeposite.text = "24%";
                break;
            case 3:
                RateDeposite.text = "12%";
                break;
            case 4:
                RateDeposite.text = "8%";
                break;
            case 5:
                RateDeposite.text = "5%";
                break;
            case 6:
                RateDeposite.text = "3%";
                break;
        }
    }
    public void Stockoptionbtn()
    {
        MainUI.instance.Displayerannouce("this future have not been supported yet!", false);
    }
    public void SourceLoanOptionChange(int value)
    {
        if (value == 0)
        {
            Ratetext.text = "9.87%";
        }
        else
        {
            Ratetext.text = "4.59%";
        }
    }
    public void setIdExchange(ushort id)
    {
        gamemng.instance.idWhoYouExChangeItem = id;
       DisplayerExchangeUi();
        instance.AddExchange();
        instance.AddGet();
        for (int i = 0; i < 12; i++)
        {
            DropDownItemExchange.cardChose.Add(0);
            DropDownItemExchange.cardChoseGet.Add(0);
        }
    }
    [SerializeField] GameObject exchangeUI;
    public void DisplayerExchangeUi()
    {
        contentwhoexchange.gameObject.SetActive(false);
        exchangeUI.SetActive(true);
    }
    [SerializeField] private Transform items;
    [SerializeField] private Transform contentExchange;
    [SerializeField] private Transform contentGet;
    public void AddExchange()
    {
        Transform newItem = Instantiate(items, contentExchange);
        newItem.GetComponent<DropDownItemExchange>().eog = 0;
        newItem.SetSiblingIndex(contentExchange.childCount - 2);
    }
    public void AddGet()
    {
        Transform newItem = Instantiate(items, contentGet);
        newItem.GetComponent<DropDownItemExchange>().eog = 1;
        newItem.SetSiblingIndex(contentGet.childCount - 2);
    }
    [SerializeField] private GameObject DoneItemExchangeRequest;
    [SerializeField] private Transform ContentRequest;
    [SerializeField] private Transform RequestExcheangeItemObj;
    public void commitchangeitem(string result)
    {
        try
        {
            ExchangeImform newexchangeitem = JsonConvert.DeserializeObject<ExchangeImform>(result);
            if (room.You.id == newexchangeitem.IdSent)
            {
                MainUI.instance.Loading.SetActive(false);
                // back to option who exchange
                ExchangeitemBackBtn();
                MainUI.instance.Displayerannouce($"Sent request exchange item success to {gamemng.instance.room.Players.Find(item => item.id == newexchangeitem.IdReceive).Name}",false);
            }
            else
            if (room.You.id == newexchangeitem.IdReceive)
            {
                //display announc new exchange item request
                Transform newExchangrItemObj = Instantiate(RequestExcheangeItemObj, ContentRequest);
                newExchangrItemObj.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.Players.Find(item => item.id == newexchangeitem.IdSent).Name;
                newExchangrItemObj.GetComponent<Button>().AddEventListener(newexchangeitem.Id, newExchangrItemObj.gameObject, exchangeitemRequestClick, SetOjbTodelele);
            }
                gamemng.instance.exchangeItems.Add(newexchangeitem);
        }
        catch
        {
            MainUI.instance.Loading.SetActive(false);
            MainUI.instance.Displayerannouce($"{result} item was used",true);
        }
    }
    [SerializeField] GameObject ExchangeinScreen;
    [SerializeField] GameObject optionExchange;
    public void ExchangeitemBackBtn()
    {
        ExchangeinScreen.SetActive(false);
        optionExchange.SetActive(true);
        for (int i=0; i< contentExchange.childCount - 1; i++)
        {
            contentExchange.GetChild(i).GetComponent<DropDownItemExchange>().destroy();
        }
        for (int i = 0; i < contentGet.childCount - 1; i++)
        {
            contentGet.GetChild(i).GetComponent<DropDownItemExchange>().destroy();
        }
    }
    public GameObject ExchangeItemUi;
    [SerializeField] GameObject GroundCard;
    [SerializeField] GameObject[] StoreCard;
    [SerializeField] Transform ContentGroundYour;
    [SerializeField] Transform ContentCardYour;
    [SerializeField] TextMeshProUGUI Yourmoney;
    [SerializeField] Transform ContentGroundFriend;
    [SerializeField] Transform ContentCardFriend;
    [SerializeField] TextMeshProUGUI Friendmoney;
    private void exchangeitemRequestClick(byte idrequest)
    {
        ExchangeItemUi.SetActive(true);
        gamemng.instance.IdRequestExchangeItem = idrequest;
        ExchangeImform newexchangeitem =  gamemng.instance.exchangeItems.Find(item => item.Id == idrequest);
        foreach(var gry in newexchangeitem.GrousdGet)
        {
            Instantiate(GroundCard, ContentGroundYour).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gry.ToString();
        }
        for(int i=0; i<12; i++)
        {
            if(newexchangeitem.CardGet[i] > 0)
            {
                for( int j=0; j< newexchangeitem.CardGet[i]; j++)
                {
                    Instantiate(StoreCard[i], ContentCardYour);
                }
            }
        }
        if (newexchangeitem.MoneyGet != 0) Yourmoney.text = newexchangeitem.MoneyGet.ToString()+"$";
        foreach (var gry in newexchangeitem.GrousdChose)
        {
            Instantiate(GroundCard, ContentGroundFriend).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gry.ToString();
        }
        for (int i = 0; i < 12; i++)
        {
            if (newexchangeitem.CardChose[i] > 0)
            {
                for (int j = 0; j < newexchangeitem.CardChose[i]; j++)
                {
                    Instantiate(StoreCard[i], ContentCardFriend);
                }
            }
        }
        if (newexchangeitem.Money != 0) Friendmoney.text = newexchangeitem.Money.ToString()+"$";
    }
    [SerializeField] private TextMeshProUGUI NameSent;
    [SerializeField] private TextMeshProUGUI NameReceive;
    [SerializeField] private Transform contentGroundSent;
    [SerializeField] private Transform contentCardSent;
    [SerializeField] private TextMeshProUGUI MoneySent;
    [SerializeField] private Transform contentGroundRecevie;
    [SerializeField] private Transform contentCardReceive;
    [SerializeField] private TextMeshProUGUI ReceiveMoney;
    public GameObject historyObj = null;
    public void HistoryExchangeClick(ExchangeImform exchangeinform)
    {
        ClearHisToryUI();
        if (historyObj != null)
        {
            historyObj.GetComponent<Image>().color = new Color32(41, 41, 41, 255);
        }
        NameSent.text = room.Players.Find(item => item.id == exchangeinform.IdSent).Name;
        NameReceive.text = room.Players.Find(item => item.id == exchangeinform.IdReceive).Name;
        if (exchangeinform.GrousdChose.Count > 0)
        {
            foreach (byte gr in exchangeinform.GrousdChose)
            {
                Instantiate(GroundCard, contentGroundSent).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gr.ToString();
            }
        }
        if (exchangeinform.GrousdGet.Count > 0)
        {
            foreach (byte gr in exchangeinform.GrousdGet)
            {
                Instantiate(GroundCard, contentGroundRecevie).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gr.ToString();
            }
        }
        for (int i = 0; i < 12; i++)
            {
                if (exchangeinform.CardGet[i] > 0)
                {
                    for (int j = 0; j < exchangeinform.CardGet[i]; j++)
                    {
                        Instantiate(StoreCard[i], contentCardReceive);
                    }
                }
                if (exchangeinform.CardChose[i] > 0)
                {
                    for (int j = 0; j < exchangeinform.CardChose[i]; j++)
                    {
                        Instantiate(StoreCard[i], contentCardSent);
                    }
                }
            }
            if (exchangeinform.Money != 0)
            {
                MoneySent.text = exchangeinform.Money.ToString();
            }
            if (exchangeinform.MoneyGet != 0)
            {
                ReceiveMoney.text = exchangeinform.MoneyGet.ToString();
            }
    }
    public void ClearHisToryUI()
    {
        foreach (Transform ch in contentGroundSent) Destroy(ch.gameObject);
        foreach (Transform ch in contentCardSent) Destroy(ch.gameObject);
        foreach (Transform ch in contentGroundRecevie) Destroy(ch.gameObject);
        foreach (Transform ch in contentCardReceive) Destroy(ch.gameObject);
        MoneySent.text = "0$";
        ReceiveMoney.text = "0$";
    }
    private void SetOjbTodelele(GameObject exchangrequest)
    {
        BankManager.ExchangerequestBtnToDelete = exchangrequest;
    }
    public void ClearExchangeItemUI()
    {
        foreach (Transform ch in ContentGroundYour) Destroy(ch.gameObject);
        foreach (Transform ch in ContentCardYour) Destroy(ch.gameObject);
        foreach (Transform ch in ContentGroundFriend) Destroy(ch.gameObject);
        foreach (Transform ch in ContentCardFriend) Destroy(ch.gameObject);
        Yourmoney.text = "0$";
        Friendmoney.text = "0$";
    }
    [SerializeField] private Transform contentYourbill;
    [SerializeField] private Transform yourbillprefab;
    [SerializeField] private Transform RequestLoanpref;
    [SerializeField] private GameObject LoanUiScreen;
    [SerializeField] private GameObject optionBankrequest;
    public void commitLoan(Message message)
    {
        string json = message.GetString();
        bool result = message.GetBool();
        LoanDetail newLoan = JsonConvert.DeserializeObject<LoanDetail>(json);
        if (gamemng.instance.room.You.id== newLoan.PartieA)
        {
                MainUI.instance.Loading.SetActive(false);
            // Bank loan success
            if (result)
            {
                // bank loan
                if (message.GetBool())
                {
                    newLoan.Isreal = true;
                    room.You.addCash(newLoan.Amount);
                    room.You.addloan(newLoan.Amount);
                    Money.text = room.You.Cash() - room.You.Loan() + "$";
                    cash.text = room.You.Cash().ToString() + "$";
                    loan.text = room.You.Loan().ToString() + "$";
                    // create bill
                    Transform Yourbill = Instantiate(yourbillprefab, contentYourbill);
                    Yourbill.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Repay loan";
                    Yourbill.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Bank";
                    Yourbill.GetChild(2).GetComponent<TextMeshProUGUI>().text = newLoan.Amount.ToString();
                    Yourbill.GetComponent<Button>().AddEventListener(newLoan.Id, Yourbill.gameObject, YourBillClick, deleteBill);
                    MainUI.instance.Displayerannouce($"You loan successfully {newLoan.Amount}$ from bank", false);
                }
                //member loan success
                else
                {
                    MainUI.instance.Displayerannouce($"Sent request loan to {room.Players.Find(item => item.id == newLoan.PartieB).Name}", false);
                    optionBankrequest.SetActive(true);
                    LoanUiScreen.SetActive(false);
                }
                    room.loans.Add(newLoan);
            }
            // PartieB dont have enough money
            else
            {
                MainUI.instance.Displayerannouce("Friend don't have enough money",true );  
            }
        }
        else
        // receive loan request
            if(room.You.id == newLoan.PartieB)
        {
            Debug.Log("here");
            Transform newLoanRequest = Instantiate(RequestLoanpref, ContentRequest);
            newLoanRequest.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.Players.Find(item => item.id == newLoan.PartieA).Name;
                    gamemng.instance.room.loans.Add(newLoan);
            newLoanRequest.GetComponent<Button>().AddEventListener(newLoan,newLoanRequest.gameObject,LoanRequestClick, deleteLoanRequest);
        }
    }
    [SerializeField] private GameObject LoanRequestUI;
    [SerializeField] private TextMeshProUGUI nameLoanUI;
    [SerializeField] private TextMeshProUGUI periodLoanUI;
    [SerializeField] private TextMeshProUGUI amountLoanUI;
    [SerializeField] private TMP_InputField rateLoanUI;
    private void LoanRequestClick(LoanDetail loan)
    {
        LoanRequestUI.SetActive(true);
        nameLoanUI.text = gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).Name;
        periodLoanUI.text = $"{loan.Period} year";
        amountLoanUI.text = $"{loan.Amount}$";
        rateLoanUI.text = loan.Rate.ToString();
        BankManager.ischangeRateLoan = false;
        gamemng.instance.idRequestLoan = loan.Id;
    }
    private GameObject loanrequesttodelete;
    private void deleteLoanRequest(GameObject requestloan)
    {
        loanrequesttodelete = requestloan;
    }
    [SerializeField] private GameObject yourBillUi;
    [SerializeField] private GameObject TranferUI;
    [SerializeField] private TextMeshProUGUI YourName;
    [SerializeField] private TextMeshProUGUI YourNumber;
    [SerializeField] private TextMeshProUGUI Friendname;
    [SerializeField] private TextMeshProUGUI FriendNumber;
    [SerializeField] private TextMeshProUGUI amountTranfer;
    [SerializeField] private TMP_InputField DescriptionTranfer;
    public void rateLoanChange(string value)
    {
        BankManager.ischangeRateLoan = true;
    }
    private void YourBillClick(byte idloan)
    {
        LoanDetail newloan = gamemng.instance.room.loans.Find(item => item.Id == idloan);
        yourBillUi.SetActive(false);
        TranferUI.SetActive(true);
        PlayerInRomm You = gamemng.instance.room.You;
            YourName.text = You.Name;
            YourNumber.text = "1903 737 942 " + You.idserver.ToString();
            amountTranfer.text = newloan.Amount.ToString()+"$";
            DescriptionTranfer.text = $"Pay off the {newloan.Amount}$ debt!";
            gamemng.instance.IdRePayLoan = newloan.Id;
        if (newloan.PartieB != 0)
        {
            PlayerInRomm Friend = gamemng.instance.room.Players.Find(item => item.id == newloan.PartieB);
            Friendname.text = Friend.Name;
            FriendNumber.text = "1903 737 942 " + Friend.idserver.ToString();
        }
        else
        {
            Friendname.text = "Bank";
            FriendNumber.text = "1903 737 942 9999";
        }
        gamemng.instance.IdRePayLoan = newloan.Id;
    }
    private void deleteBill(GameObject billOBJ)
    {
        billTodelete = billOBJ;
    }
    GameObject billTodelete;
    [SerializeField] private GameObject ReLoanUI;
    [SerializeField] private TextMeshProUGUI SourceReloanUI;
    [SerializeField] private TextMeshProUGUI PeriodReloanUI;
    [SerializeField] private TextMeshProUGUI AmoutReloanUI;
    [SerializeField] private TMP_InputField rateReloan;
    public void ReplyLoan(Message mess)
    {
        float idrequestloan = mess.GetByte();
        LoanDetail loan = room.loans.Find(item => item.Id == idrequestloan);
        bool result = mess.GetBool();
        if (gamemng.instance.room.You.id == loan.PartieA)
        {
            if (result)
            {
                bool ischangeRate = mess.GetBool();
                if (ischangeRate)
                {
                    float rate = mess.GetFloat  ();
                    MainUI.instance.Displayerannouce($"{room.Players.Find(item => item.id == loan.PartieB).Name} change rate to loan", false);
                    SourceReloanUI.text = room.Players.Find(item => item.id == loan.PartieB).Name;
                    PeriodReloanUI.text = loan.Period.ToString();
                    AmoutReloanUI.text = loan.Amount.ToString();
                    rateReloan.text = rate.ToString() + "%";
                    loan.Rate = rate;
                    ReLoanUI.SetActive(true);
                    BankManager.idLoanToReLoan = loan.Id;
                }
                else
                {
                    MainUI.instance.Displayerannouce($"You have just been approved  for a loan {loan.Amount}", false);
                    loan.Isreal = true;
                   room.You.addCash(loan.Amount);
                    room.You.addloan(loan.Amount);
                    Money.text = room.You.Cash() - room.You.Loan() + "$";
                    cash.text = room.You.Cash().ToString() + "$";   
                    this.loan.text = room.You.Loan().ToString() + "$";
                    // new bill
                    Transform Yourbill = Instantiate(yourbillprefab, contentYourbill);
                    Yourbill.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Repay loan";
                    Yourbill.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).Name;
                    Yourbill.GetChild(2).GetComponent<TextMeshProUGUI>().text = loan.Amount.ToString();
                    Yourbill.GetComponent<Button>().AddEventListener(loan.Id, Yourbill.gameObject, YourBillClick, deleteBill);
                    // update home banking
                    cash.text = room.You.Cash().ToString() + "$";
                    this.loan.text = room.You.Loan().ToString() + "$";
                }
            }
            else
            {
                MainUI.instance.Displayerannouce($"your loan request be refused by {gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).Name}", true);
                gamemng.instance.room.loans.Remove(loan);
            }
        }
        else
            if (gamemng.instance.room.You.id == loan.PartieB)
        {
            if (result)
            {
                if (!mess.GetBool())
                {
                    gamemng.instance.room.You.addCash(-loan.Amount);
                    gamemng.instance.room.You.adddeposite(loan.Amount);
                    MainUI.instance.Displayerannouce($"You lent {gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).Name} {loan.Amount}$",false);
                    loan.Isreal = true;
                    //update home banking
                    cash.text = room.You.Cash().ToString() + "$";
                    deposite.text = room.You.Loan().ToString() + "$";
                }
                else
                {
                    loan.Rate = mess.GetFloat();
                   
                }
            }
            else
            {
                gamemng.instance.room.loans.Remove(loan);
            }
            Destroy(loanrequesttodelete);
            LoanRequestUI.SetActive(false);
            MainUI.instance.Loading.SetActive(false);
        }
    }
    public void Reloan(Message mess)
    {
        byte idloan = mess.GetByte();
        LoanDetail loan = gamemng.instance.room.loans.Find(item => item.Id == idloan);
        if (mess.GetBool())
        {
            gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).addCash(loan.Amount);
            gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).addloan(loan.Amount);
            gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).addCash(-loan.Amount);
            gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).adddeposite(loan.Amount);
            //Debug.Log(gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).Money() + ";" + gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).Money());
            loan.Isreal = true;
            if (gamemng.instance.room.You.id == loan.PartieA)
            {
                ReLoanUI.SetActive(false);
                MainUI.instance.Loading.SetActive(false);
                MainUI.instance.Displayerannouce($"You loan {loan.Amount} from {gamemng.instance.room.Players.Find(item =>item.id == loan.PartieB).Name}",false);
                // new bill
                Transform Yourbill = Instantiate(yourbillprefab, contentYourbill);
                Yourbill.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Repay loan";
                Yourbill.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = gamemng.instance.room.Players.Find(item => item.id == loan.PartieB).Name;
                Yourbill.GetChild(2).GetComponent<TextMeshProUGUI>().text = loan.Amount.ToString();
                Yourbill.GetComponent<Button>().AddEventListener(loan.Id, Yourbill.gameObject, YourBillClick, deleteBill);
                // update Home banking
                cash.text = room.You.Cash().ToString() + "$";
                this.loan.text = room.You.Loan().ToString() + "$";
            }
            else
            {
                MainUI.instance.Displayerannouce($"You lent {gamemng.instance.room.Players.Find(item => item.id == loan.PartieA).Name} {loan.Amount}$ ", false);
                // update home banking
                cash.text = room.You.Cash().ToString() + "$";
                deposite.text = room.You.Loan().ToString() + "$";
            }
        }
        else
        {
            if (gamemng.instance.room.You.id == loan.PartieA)
            {
                ReLoanUI.SetActive(false);
                MainUI.instance.Loading.SetActive(false);
            }
            gamemng.instance.room.loans.Remove(loan);
        }
    }
    [SerializeField] GameObject DepositeUI;
    public void Deposite(Message message)
    {
        if (message.GetBool())
        {
            string json = message.GetString();
            DepositeDetail nwedeposite = JsonConvert.DeserializeObject<DepositeDetail>(json);
            room.You.addCash(-nwedeposite.amount);
            room.You.adddeposite(nwedeposite.amount);
            MainUI.instance.Displayerannouce($"you deposited {nwedeposite.amount} for {nwedeposite.period} year", false);
            DepositeUI.SetActive(false);
            optionBankrequest.SetActive(true);
            // home banking
            cash.text = room.You.Cash().ToString()+"$";
            deposite.text = room.You.Deposite().ToString() + "$";

        }
        else
        {
            MainUI.instance.Displayerannouce("You not enough money to Deposite", true);
        }
        MainUI.instance.Loading.SetActive(false);
    }
    private IEnumerator DisplayAnnouc(GameObject announc)
    {
        announc.SetActive(true);
        yield return new WaitForSeconds(6);
        announc.SetActive(false);
    }
}
