using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PlayerSkillInventory
{
    [Header("Skill Inventory")]
    [SerializeField]
    private List<SkillSlot> activeSkills;
    [SerializeField]
    private int activeCapacity;
    [SerializeField]
    private List<PassiveSlot> passiveSkills;
    [SerializeField]
    private int passiveCapacity;


    [Header("Skill Equip")]
    [SerializeField]
    private List<SkillSlot> skillequip;
    private int equipCapacity = 4;

    //property
    public List<SkillSlot> ActiveSkills => activeSkills;
    public int ActiveCapacity => activeCapacity;
    public List<PassiveSlot> PassiveSkills => passiveSkills;
    public int PassiveCapacity => passiveCapacity;
    public List<SkillSlot> Skillequip => skillequip;
    public int EquipCapacity => equipCapacity;

    //delitgate
    public Action OnChangedActiveSkill;
    public Action OnChangedPassiveSkill;

    public void Init()
    {
        if (activeSkills != null && activeSkills.Count > 0) return;

        activeSkills = new List<SkillSlot>(activeCapacity);
        skillequip = new List<SkillSlot>(equipCapacity);
        passiveSkills = new List<PassiveSlot>(passiveCapacity);
        for (int i = 0; i < activeCapacity; i++)
        {
            activeSkills.Add(new SkillSlot());
        }
        for (int i = 0; i < passiveCapacity; i++)
        {
            passiveSkills.Add(new PassiveSlot());
        }

        for (int i = 0; i < equipCapacity; i++)
        {
            skillequip.Add(new SkillSlot());
        }
    }
    public bool AddPassiveSkill(PassiveSkill passiveSkill)
    {
        PassiveSlot emptySlot = passiveSkills.Find(s => s.IsEmpty());
        if(emptySlot !=null)
        {
            emptySlot.passiveSkill = passiveSkill;
            OnChangedPassiveSkill.Invoke();
            return true;
        }
        return false;
    }
    public bool AddActiveSkill(Skill skill)
    {
        SkillSlot emptySlot = activeSkills.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.skill = skill;
            OnChangedActiveSkill?.Invoke();
            return true;
        }
        return false;
    }
    public void Clear()
    {
        for (int i = 0; i < activeCapacity; i++)
        {
            activeSkills[i].Clear();
        }
        for (int i =0; i<passiveCapacity; i++)
        {
            passiveSkills[i].Clear();
        }
        OnChangedActiveSkill?.Invoke();
        OnChangedPassiveSkill?.Invoke();
    }
    public void RegistActiveSkill(SkillSlot slot, int index)
    {
        if (slot.IsEmpty()) return;
        if (CheckDuplication(slot)) return;
        skillequip[index].Clear();
        skillequip[index].skill = slot.skill;
        OnChangedActiveSkill?.Invoke();
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
        OnChangedActiveSkill?.Invoke();
    }
}
