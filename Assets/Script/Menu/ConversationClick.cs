using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversationClick : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private GameObject state;
    [SerializeField] private TextMeshProUGUI id;
   public void click()
    {
        gamemng.instance.NameChat = name.text;
        gamemng.instance.stateChat = state.activeSelf;
        gamemng.instance.idChatRoom = int.Parse(id.text);
    }
}
