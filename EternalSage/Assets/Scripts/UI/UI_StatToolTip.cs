using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;


    public void ShowStatToolTip(string _text)
    {
        statDescription.text = _text;
        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        gameObject.SetActive(false);
    }

}
