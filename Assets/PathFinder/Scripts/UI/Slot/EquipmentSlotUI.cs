using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : SlotUI , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] 
    private EquipmentType type;
    private int index;

    InventoryUI ui;

    public int SlotIndex => index;
    private void Start()
    {
        index = (int)type;
        ui = GetComponentInParent<InventoryUI>();
        player.Inventory.OnEquipmentChanged += UpdateUI;
        UpdateUI();
    }
    public EquipmentsSlot GetSlotData()
    {
        return player.Inventory.Equipments[index];
    }

    public override void UpdateUI()
    {
        var slotData = player.Inventory.Equipments[index];
        if (!slotData.IsEmpty())
        {
            icon.sprite = slotData.item.Data.Sprite;
            icon.color = Color.white;
            group.alpha = 1;
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            group.alpha = 0;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;
        ui?.ExplainReomote(true);
        ui?.UpdateExplain(GetSlotData());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;
        ui?.ExplainReomote(false);
    }
}