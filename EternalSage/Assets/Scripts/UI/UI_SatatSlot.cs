using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SatatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] Stats_Type statsType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

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
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUi()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statsType).GetValue().ToString();

            switch (statsType)
            {
                case Stats_Type.ST_Strengh:
                    break;
                case Stats_Type.ST_Agility:
                    break;
                case Stats_Type.ST_Intelligence:
                    break;
                case Stats_Type.ST_Vitality:
                    break;
                case Stats_Type.ST_Damage:
                    statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case Stats_Type.ST_CritChance:
                    statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case Stats_Type.ST_critPower:
                    statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
                    break;
                case Stats_Type.ST_Armor:
                    break;
                case Stats_Type.ST_Health:
                    statValueText.text = playerStats.GetMaxHealthValue().ToString();
                    break;
                case Stats_Type.ST_MagicRes:
                    statValueText.text = (playerStats.magicalResistance.GetValue() + 
                        (playerStats.intelligence.GetValue() * playerStats.intFactor)).ToString();
                    break;
                case Stats_Type.ST_Evasion:
                    statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
                    break;
                case Stats_Type.ST_FireDmg:
                    break;
                case Stats_Type.ST_MetalDmg:
                    break;
                case Stats_Type.ST_IceDmg:
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.ToggleStatToolTip(true);
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
