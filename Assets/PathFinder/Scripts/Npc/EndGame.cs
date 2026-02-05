using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour, IInteractable
{
    public void Interact(Player player)
    {
        //게임 종료
        Debug.Log("게임종료");
    }
}
