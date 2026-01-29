using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUIPresentor : MonoBehaviour
{
    [SerializeField]
    private StatusValueUI statusValueUI;
    [SerializeField]
    private StatusAddableUI statusAddableUI;


    private Player player;
    private void Start()
    {
        player = GameManager.instance.Player;
        statusAddableUI.ResistEvent(player.StatusSystem);
        player.StatusSystem.OnStatChanged += RefreshUI;
        RefreshUI();
    }
    private void OnDestroy()
    {
        player.StatusSystem.OnStatChanged -= RefreshUI;
    }
    public void RefreshUI()
    {
        statusValueUI.RefreshUI(player.StatusSystem);
        statusAddableUI.RefreshUI(player.StatusSystem,player.LevelSystem);
    }
}