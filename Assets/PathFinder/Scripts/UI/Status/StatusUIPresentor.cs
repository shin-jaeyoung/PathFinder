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
        player.StatusSystem.OnStatChanged += RefreshUI;
        player.LevelSystem.OnExpChanged += RefreshLevelUI;
        HiddenManager.instance.OnEndHidden += RefreshHiidenUI;
        RefreshUI();
        RefreshLevelUI();
        RefreshHiidenUI();
    }
    private void OnDestroy()
    {
        player.StatusSystem.OnStatChanged -= RefreshUI;
        player.LevelSystem.OnExpChanged -= RefreshLevelUI;
        HiddenManager.instance.OnEndHidden -= RefreshHiidenUI;
    }
    public void RefreshUI()
    {
        statusValueUI.RefreshUI(player.StatusSystem);
        statusAddableUI.RefreshUI(player.StatusSystem,player.LevelSystem);
        statusAndHiddenView.RefreshHpText(player);
    }
    public void RefreshLevelUI()
    {
        statusAndHiddenView.RefreshExpText(player);
    }
    public void RefreshHiidenUI()
    {
        statusAndHiddenView.RefreshHiddenCount(HiddenManager.instance);
    }
    
}