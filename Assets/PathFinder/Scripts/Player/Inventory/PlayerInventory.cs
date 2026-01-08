using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    [Header("Item Inventory")]
    [SerializeField]
    private List<InventorySlot> inventory;
    [SerializeField]
    private int capacity;
    [Header("Equipment Inventory")]
    [SerializeField]
    private List<InventorySlot> equipments;
    [SerializeField]
    private int equipmentsCapacity;
    [Header("Gold")]
    [SerializeField]
    private int gold;

    //property
    public List<InventorySlot> Inventory => inventory;
    public int Capacity => capacity;
    public List<InventorySlot> Equipments => equipments;
    public int Gold => gold;

    //deligate
    public Action OnInventoryChaneged;
    public Action OnEquipmentChanged;
    public Action OnGoldChanged;
    public void Init()
    {
        if (inventory != null && inventory.Count > 0) return;

        inventory = new List<InventorySlot>(capacity);
        equipments = new List<InventorySlot>(equipmentsCapacity);
        for (int i = 0; i < capacity; i++)
        {
            inventory.Add(new InventorySlot());
        }
        for (int i = 0; i < equipmentsCapacity; i++)
        {
            equipments.Add(new InventorySlot());
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
                OnInventoryChaneged?.Invoke();
                return true;
            }
        }
        InventorySlot emptySlot = inventory.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.count = amount;
            OnInventoryChaneged?.Invoke();
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
                OnInventoryChaneged?.Invoke();
                return true;
            }
            Debug.Log("수량이 충분 x");
            return false;
        }
        else
        {
            slot.Clear();

            OnInventoryChaneged?.Invoke();
            return true;
        }
    }
    public bool AddEquipment(InventorySlot slot, int index)
    {
        if(slot.IsEmpty()) return false;
        if (equipments[index].IsEmpty())
        {
            equipments[index].item = slot.item;
            equipments[index].count = 1;
        }
        else
        {
            AddItem(equipments[index].item);
            equipments[index].item = slot.item;
            equipments[index].count = 1;
        }
        slot.Clear();
        OnInventoryChaneged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return false;
    }
    public bool RemoveEquipment( int index)
    {
        if (equipments[index].IsEmpty()) return false;

        AddItem(equipments[index].item);
        equipments[index].Clear();
        OnInventoryChaneged?.Invoke();
        OnEquipmentChanged?.Invoke();
        return true;
    }
}
