using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusAndHiddenView : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]
    private TextMeshProUGUI hpText;
    [Header("Exp")]
    [SerializeField]
    private TextMeshProUGUI expText;
    [Header("Hidden")]
    [SerializeField]
    private TextMeshProUGUI hiddenCount;

    public void RefreshHpText(Player player)
    {
        hpText.text = $"{player.StatusSystem.Stat[PlayerStatType.CurHp]} / {player.StatusSystem.FinalStat[PlayerStatType.MaxHp]}";
    }

    public void RefreshExpText(Player player)
    {
        expText.text = $"{player.LevelSystem.CurExp} / {player.LevelSystem.MaxExp}";
    }
    public void RefreshHiddenCount(HiddenManager hiddenmanager)
    {
        hiddenCount.text = $"{hiddenmanager.CheckEndHiddenCount()} / {hiddenmanager.ChechkHiddenCount()}";
    }
}
