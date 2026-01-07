using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int count;

    public bool IsEmpty()
    {
        if(item ==null  || count <=0)
        {
            return true;
        }
        return false;
    }
    public void Clear()
    {
        item = null;
        count = 0;
    }
}
