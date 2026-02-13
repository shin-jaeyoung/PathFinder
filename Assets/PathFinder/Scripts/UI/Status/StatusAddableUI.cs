using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusAddableUI : MonoBehaviour
{
    [Header("Text for Value")]
    [SerializeField]
    private TextMeshProUGUI strValue;
    [SerializeField]
    private TextMeshProUGUI dexValue;
    [SerializeField]
    private TextMeshProUGUI conValue;
    [SerializeField]
    private TextMeshProUGUI levelPointValue;

    [Header("Button for AddStat")]
    [SerializeField]
    private Button strButton;
    [SerializeField]
    private Button dexButton;
    [SerializeField]
    private Button conButton;

    public void RefreshUI(string strValue,string dexValue, string conValue)
    {
        this.strValue.text = strValue;
        this.dexValue.text = dexValue;
        this.conValue.text = conValue;

    }
    public void RefreshUILevelPoint(string levelPoint)
    {
        levelPointValue.text = levelPoint;
    }
    public void ResistEvent(PlayerStatusSystem stat)
    {
        strButton.onClick.RemoveAllListeners();
        dexButton.onClick.RemoveAllListeners();
        conButton.onClick.RemoveAllListeners();

        strButton.onClick.AddListener(() =>
        {
            stat.AddStat(PlayerStatType.STR, 1);
        });
        dexButton.onClick.AddListener(() =>
        {
            stat.AddStat(PlayerStatType.DEX, 1);
        });
        conButton.onClick.AddListener(() =>
        {
            stat.AddStat(PlayerStatType.CON, 1);
        });
    }

}
