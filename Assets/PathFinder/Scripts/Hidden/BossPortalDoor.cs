using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortalDoor : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxcollider;

    private bool isOpen = false;

    private void Update()
    {
        if (!isOpen && HiddenManager.instance.HiddenDic[99].State == HiddenState.End)
        {
            isOpen = true;
            boxcollider.isTrigger = true;
        }
    }
    private void OnDestroy()
    {
        isOpen = false;
        boxcollider.isTrigger = false;
    }
}
