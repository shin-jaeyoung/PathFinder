using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot : Slot
{
    public Item item;

    public override bool IsEmpty()
    {
        if(item ==null  || count <= 0)
        {
            return true;
        }
        return false;
    }
    public override void Clear()
    {
        item = null;
        count = 0;
    }
}
