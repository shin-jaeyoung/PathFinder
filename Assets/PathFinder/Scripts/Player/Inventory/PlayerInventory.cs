using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Item Inventory")]
    [SerializeField]
    private List<InventorySlot> inventory;
    [SerializeField]
    private int capacity;
    [Header("Equipment Inventory")]
    [SerializeField]
    private List<InventorySlot> equipments;



    public List<InventorySlot> Inventory => inventory;
    public int Capacity => capacity;

    private void Awake()
    {

        inventory = new List<InventorySlot>(capacity);
        equipments = new List<InventorySlot>(equipments);
        for (int i = 0; i < capacity; i++)
        {
            inventory.Add(new InventorySlot());
        }
    }


    public bool AddItem(Item item, int amount = 1)
    {

        if (item is ExtraItem extra)
        {
            InventorySlot existingSlot = inventory.Find(s => s.item == item && s.count < extra.CapacityInSlot);
            if (existingSlot != null)
            {
                existingSlot.count += amount;
                return true;
            }
        }
        InventorySlot emptySlot = inventory.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.count = amount;
            return true;
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
        return false;
    }
    public bool RemoveItem(InventorySlot slot , int amount = 1)
    {
        if(slot.IsEmpty()) return false;
        if(slot.item is ExtraItem extra)
        {
            if(slot.count>amount)
            {
                slot.count -= amount;
                if(slot.count ==0)
                {
                    slot.Clear();
                }
                return true;
            }
            Debug.Log("수량이 충분 x");
            return false;
        }
        else
        {
            slot.Clear();
            return true;
        }
    }
}
