using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
public class LoginUI : MonoBehaviour
{
    public static LoginUI instance;

    [SerializeField] private Login login;
    [SerializeField] private TMP_InputField EmailLogin;
    [SerializeField] private TMP_InputField PassLogin;
    [SerializeField] private Image WarnEmail;
    [SerializeField] private Image WarnPass;
    [SerializeField] private GameObject Twnoemail;
    [SerializeField] private GameObject Twwrongpass;
    private void Start()
    {
        instance = this;
    }

    public void InputOutClick(TMP_InputField Input)
    {
        Input.placeholder.gameObject.SetActive(true);
    }
    public void InputClick(TMP_InputField Input)
    {
        Input.placeholder.gameObject.SetActive(false);
        WarnEmail.color = new Color(255, 255, 255);
        WarnPass.color = new Color(255, 255, 255);
        Twnoemail.SetActive(false);
        Twwrongpass.SetActive(false);
    }
    public void SignBtn()
    {
        SceneManager.LoadScene("SiginScene");
    }
    public void SubmitBtn()
    {
        int d = 0;
        if (EmailLogin.text == "") { WarnEmail.color = new Color(255, 0, 0); d++; }
        if (PassLogin.text == "") { WarnPass.color = new Color(255, 0, 0); d++;  }
        if (d == 0) login.SendCheck(EmailLogin.text, PassLogin.text);
    }

    public  void loginUi(string result,ushort id)
    {
        if (result == "None email" || result == "wrong password")
        {
            WarnEmail.color = new Color(255, 0, 0);
            WarnPass.color = new Color(255, 0, 0);
            if (result == "None email") Twnoemail.SetActive(true);
            if (result == "wrong password") Twwrongpass.SetActive(true);
        } else 
        {
            Debug.Log(result);
            gamemng.instance.playerinf = JsonConvert.DeserializeObject<PlayerInf>(result);
            gamemng.instance.Id = id;
            SceneManager.LoadScene("MenuScene");
            Debug.Log(gamemng.instance.playerinf.Id);
        }
    }
}
    
