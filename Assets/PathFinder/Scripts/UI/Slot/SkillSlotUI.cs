using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlotUI : SlotUI ,IPointerClickHandler
{
    private int slotIndex;
    [SerializeField]
    private bool isEquipslot;


    public int SlotIndex => slotIndex;
    public void SetIndex(int index)
    {
        slotIndex = index;
    }
    public SkillSlot GetSlotData()
    {
        if(isEquipslot)
        {
            return player.Skills.Skillequip[SlotIndex];
        }
        
        return player.Skills.ActiveSkills[slotIndex];
        
    }

    private void Start()
    {
        player.Skills.OnChangedActiveSkill += UpdateUI;
        UpdateUI();
    }
    public override void OnDrop(PointerEventData eventData)
    {
        SkillSlotUI draggedSlot = eventData.pointerDrag?.GetComponent<SkillSlotUI>();
        if(draggedSlot != null)
        {
            //드래그한애가 스킬인벤이면
            if (!draggedSlot.isEquipslot)
            {
                //드랍 장소가 장착슬롯이면
                if (isEquipslot)
                {
                    player.Skills.RegistActiveSkill(draggedSlot.GetSlotData(), slotIndex);
                }
                iconRect.anchoredPosition = Vector2.zero;
                return;
            }
            //드래그한 애가 장착슬롯이면
            else
            {
                //드랍 장소가 장착슬롯이면
                if(isEquipslot)
                {
                    //스왑
                    if (draggedSlot == this) return; 
                    var equipList = player.Skills.Skillequip;
                    SkillSlot temp = equipList[slotIndex];
                    equipList[slotIndex] = equipList[draggedSlot.slotIndex];
                    equipList[draggedSlot.slotIndex] = temp;
                    player.Skills.OnChangedActiveSkill?.Invoke();
                }
                //드랍 장소가 스킬 인벤이면
                else
                {
                    player.Skills.UnregistSkill(draggedSlot.slotIndex);
                }
            }
            
            
        }
    }
        
    

    public override void UpdateUI()
    {
        SkillSlot slotData;

        if (isEquipslot)
        {
            slotData = player.Skills.Skillequip[slotIndex];
        }
        else
        {
            slotData = player.Skills.ActiveSkills[slotIndex];
        }
        if (!slotData.IsEmpty())
        {
            icon.sprite = slotData.skill.Data.Icon;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;
        SkillInventoryUI ui = GetComponentInParent<SkillInventoryUI>();
        ui.UpdateExplain(GetSlotData());
    }
}
