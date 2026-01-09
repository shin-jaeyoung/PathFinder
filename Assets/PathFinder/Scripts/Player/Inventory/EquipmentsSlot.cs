using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentsSlot : Slot
{
    public Equipment item;

    public override void Clear()
    {
        item = null;
        count = 0;
    }

    public override bool IsEmpty()
    {
        if (item == null || count <= 0)
        {
            return true;
        }
        return false;
    }
}
