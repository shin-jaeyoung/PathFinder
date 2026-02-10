using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveSkillSlotUI : SlotUI ,IPointerClickHandler
{
    private int slotIndex;
    public int SlotIndex => slotIndex;
    public void SetIndex(int index)
    {
        slotIndex = index;
    }
    public PassiveSlot GetSlotData()
    {
        return player.Skills.PassiveSkills[slotIndex];
    }
    private void Start()
    {
        player.Skills.OnChangedPassiveSkill += UpdateUI;
        UpdateUI();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        
    }

    public override void UpdateUI()
    {
        if (player == null) player = GameManager.instance.Player;
        PassiveSlot slot = player.Skills.PassiveSkills[slotIndex];
        if (!slot.IsEmpty())
        {
            icon.sprite = slot.passiveSkill.Data.Icon;
            icon.color = Color.white;
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;
        SkillInventoryUI ui = GetComponentInParent<SkillInventoryUI>();
        ui.UpdateExplain(GetSlotData());
    }
}
