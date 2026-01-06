using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ExtraItem", menuName =("Item/ExtraItem"))]
public class ExtraItem : Item ,IMultiSellable
{
    [SerializeField]
    private int capacityInSlot;

    public int CapacityInSlot => capacityInSlot;
    public int GetTotalPrice(int count)
    {
        return GetPrice() * count;
    }
}
