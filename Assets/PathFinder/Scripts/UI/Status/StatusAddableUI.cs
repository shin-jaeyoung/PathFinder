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

    public void RefreshUI(PlayerStatusSystem stat,PlayerLevelSystem levelSystem)
    {
        strValue.text = stat.Stat[PlayerStatType.STR].ToString();
        dexValue.text = stat.Stat[PlayerStatType.DEX].ToString();
        conValue.text = stat.Stat[PlayerStatType.CON].ToString();
        levelPointValue.text = levelSystem.LevelPoint.ToString();
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
