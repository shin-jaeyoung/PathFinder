using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInvenUI : MonoBehaviour
{
    public List<ShopInvenSlotUI> shopinvenSlots;

    private void Start()
    {
        for(int  i = 0; i < shopinvenSlots.Count; i++)
        {
            shopinvenSlots[i].slotIndex = i;
        }
    }
    public void RefreshUI(List<InventorySlot> inventory)
    {
        for(int i = 0; i<inventory.Count; i++)
        {
            shopinvenSlots[i].RefreshUI(inventory[i].item.Data.Sprite, inventory[i].count);
        }
    }

}
