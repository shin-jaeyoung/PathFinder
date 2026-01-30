using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObj : MonoBehaviour,IInteractable
{
    [Header("Hidden Info")]
    [SerializeField] 
    private int hiddenId;
    [SerializeField]
    private int objId;

    public void Interact(Player player)
    {
        HiddenManager.instance.ProcessHidden(hiddenId, objId, player);
    }
}
