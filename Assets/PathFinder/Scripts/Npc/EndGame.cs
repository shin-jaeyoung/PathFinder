using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        //여기에 저장기능 넣으면 될듯?
        GameManager.instance.SaveGame();
        //게임 종료
        Debug.Log("게임종료");
        Application.Quit();
    }
}
