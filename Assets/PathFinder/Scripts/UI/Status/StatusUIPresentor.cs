using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUIPresentor : MonoBehaviour
{
    [SerializeField]
    private StatusValueUI statusValueUI;
    [SerializeField]
    private StatusAddableUI statusAddableUI;
    [SerializeField]
    private StatusAndHiddenView statusAndHiddenView;

    private Player player;
    private void Start()
    {
        player = GameManager.instance.Player;
        statusAddableUI.ResistEvent(player.StatusSystem);
        player.StatusSystem.OnStatChanged += RefreshStatusValueUI;
        player.LevelSystem.OnExpChanged += RefreshExpUI;
        player.LevelSystem.OnLevelPointChanged += RefreshLevelPointUI;
        HiddenManager.instance.OnEndHidden += RefreshHiidenUI;
        RefreshStatusValueUI();
        RefreshExpUI();
        RefreshLevelPointUI();
        RefreshHiidenUI();
    }
    private void OnDestroy()
    {
        player.StatusSystem.OnStatChanged -= RefreshStatusValueUI;
        player.LevelSystem.OnExpChanged -= RefreshExpUI;
        player.LevelSystem.OnLevelPointChanged -= RefreshLevelPointUI;
        HiddenManager.instance.OnEndHidden -= RefreshHiidenUI;
    }
    public void RefreshStatusValueUI()
    {
        statusValueUI.RefreshUI(player.StatusSystem);
        statusAddableUI.RefreshUI(player.StatusSystem.Stat[PlayerStatType.STR].ToString(),
            player.StatusSystem.Stat[PlayerStatType.DEX].ToString(),
            player.StatusSystem.Stat[PlayerStatType.CON].ToString());
        statusAndHiddenView.RefreshHpText(player);
    }
    public void RefreshExpUI()
    {
        statusAndHiddenView.RefreshExpText(player);
    }
    public void RefreshLevelPointUI()
    {
        statusAddableUI.RefreshUILevelPoint(player.LevelSystem.LevelPoint.ToString());
    }
    public void RefreshHiidenUI()
    {
        statusAndHiddenView.RefreshHiddenCount(HiddenManager.instance);
    }
    
}