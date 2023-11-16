using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SatatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] Stats_Type statsType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if(statNameText != null)
        {
            statNameText.text = statName;
        }

    }


    void Start()
    {
        UpdateStatValueUi();
    }

    public void UpdateStatValueUi()
    {
    
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statsType).GetValue().ToString();
        }

    }
}
