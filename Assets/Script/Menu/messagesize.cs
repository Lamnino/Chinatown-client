using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class messagesize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform Rectcontain;
    private void Start()
    {
        Rectcontain = gameObject.GetComponent<RectTransform>();
        Rectcontain.sizeDelta = new Vector2(200, text.preferredHeight + 5);
    }
}
