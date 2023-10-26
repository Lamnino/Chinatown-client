using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Riptide;

public class SigninUI : MonoBehaviour
{
    private string result;
    [SerializeField] private Signin signin;
    [SerializeField] private List<InputField> Input;
    [SerializeField] private List<GameObject> TextWarn;
    [SerializeField] private GameObject SuccessSignin;

    public void InputClick(InputField Input)
    {
        Input.placeholder.gameObject.SetActive(false);
        HireAllWarn();
    }
    public void OutInputClick(InputField Input)
    {
        Input.placeholder.gameObject.SetActive(true);
    }
    public void SubmitBtn()
    {
        if (!CheckNoneType())
        {
            return;
        }
        if (Input[2].text != Input[3].text)
        {
            TextWarn[2].SetActive(true);
            return;
        }
        SendCheck();
    }
    private void SendCheck()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServer.Signin);
        foreach (var input in Input)
        {
            message.Add(input.text);
        }
        NetworkManager.Singleton.client.Send(message);
    }
    private bool CheckNoneType()
    {
         int d = 0;
        foreach (var input in Input)
        {
            if (input.text == "")
            {
                Image warn = input.transform.GetChild(3).GetComponent<Image>();
                warn.color = new Color(255, 0, 0); 
                d++;
            }
        }
        return (d == 0) ? true : false;
    }
    private void HireAllWarn()
    {
        foreach (var input in Input)
        {
                Image warn = input.transform.GetChild(3).GetComponent<Image>();
                warn.color = new Color(255, 255, 255);
        }
        foreach (var warn in TextWarn)
        {
            warn.SetActive(false);
        }
    }
    public void LoginBtn()
    {
        SceneManager.LoadScene("loginScene");
    }
    private void FixedUpdate()
    {
        result = Signin.result;
        if (result != "")
        {
            Debug.Log(result);
            if (result == "00")
            {
                TextWarn[0].SetActive(true);
                TextWarn[1].SetActive(true);
            }
            else if (result == "01")
            {
                TextWarn[0].SetActive(true);
            }
            else if (result == "10")
            {
                TextWarn[1].SetActive(true);
            }
            else if (result == "11")
            {
                SuccessSignin.SetActive(true);
            }
            Signin.result = "";
        }
    }



}
