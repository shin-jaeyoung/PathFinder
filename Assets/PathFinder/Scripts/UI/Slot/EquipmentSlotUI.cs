using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : SlotUI
{
    [SerializeField] 
    private EquipmentType type;
    private int index;


    public int SlotIndex => index;
    private void Start()
    {
        index = (int)type;
        player.Inventory.OnEquipmentChanged += UpdateUI;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        var slotData = player.Inventory.Equipments[index];
        if (!slotData.IsEmpty())
        {
            icon.sprite = slotData.item.Data.Sprite;
            icon.color = Color.white;
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            icon.gameObject.SetActive(false);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (draggedSlot != null)
        {
            Item item = draggedSlot.GetSlotData().item;
            if (item is Equipment equip && equip.Type == type)
            {
                player.Inventory.AddEquipment(draggedSlot.GetSlotData(), index);
            }
        }
        iconRect.anchoredPosition = Vector2.zero;
    }
}