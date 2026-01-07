using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSkillInventory
{
    [Header("Skill Inventory")]
    [SerializeField]
    private List<SkillSlot> skills;
    [SerializeField]
    private int capacity;

    [Header("Skill Equip")]
    [SerializeField]
    private List<SkillSlot> skillequip;
    private int equipCapacity = 4;


    //delitgate
    public Action OnChangedSkill;


    public void Init()
    {
        if (skills != null && skills.Count > 0) return;

        skills = new List<SkillSlot>(capacity);
        skillequip = new List<SkillSlot>(equipCapacity);
        for (int i = 0; i < capacity; i++)
        {
            skills.Add(new SkillSlot());
        }
        for (int i = 0; i < equipCapacity; i++)
        {
            skillequip.Add(new SkillSlot());
        }
    }

    public bool AddSkill(Skill skill)
    {
        SkillSlot emptySlot = skills.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.skill = skill;
            OnChangedSkill?.Invoke();
            return true;
        }
        return false;
    }
    public void Clear()
    {
        for (int i = 0; i < capacity; i++)
        {
            skills[i].Clear();
        }
        OnChangedSkill?.Invoke();
    }
    public void RegistSkill(SkillSlot slot, int index)
    {
        if (slot.IsEmpty()) return;
        if (CheckDuplication(slot)) return;
        skillequip[index].Clear();
        skillequip[index].skill = slot.skill;
        OnChangedSkill?.Invoke();
    }
    public bool CheckDuplication(SkillSlot slot)
    {
        if (slot.skill == null) return true;
        bool check = false;
        for (int i = 0; i < skillequip.Count; i++)
        {
            if (skillequip[i].skill == slot.skill)
            {
                check = true; break;
            }
        }
        return check;
    }
    public void UnregistSkill(int index)
    {
        skillequip[index].Clear();
        OnChangedSkill?.Invoke();
    }
}
