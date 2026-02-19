using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PlayerSkillInventory
{
    [Header("BasicAttack")]
    [SerializeField]
    private Skill basicAttack;
    private SkillSlot basicAttackSlot = new SkillSlot();

    [Header("Skill Inventory")]
    [SerializeField]
    private List<SkillSlot> activeSkills;
    [SerializeField]
    private int activeCapacity;
    [SerializeField]
    private List<PassiveSlot> passiveSkills;
    [SerializeField]
    private int passiveCapacity;
    [SerializeField]
    private SkillSlot dashSkill;
    [Header("Skill Equip")]
    [SerializeField]
    private List<SkillSlot> skillequip;
    private int equipCapacity = 4;

    //계산용
    private Dictionary<PlayerStatType, float> addStatus;

    //property
    public List<SkillSlot> ActiveSkills => activeSkills;
    public int ActiveCapacity => activeCapacity;
    public List<PassiveSlot> PassiveSkills => passiveSkills;
    public int PassiveCapacity => passiveCapacity;
    public List<SkillSlot> Skillequip => skillequip;
    public int EquipCapacity => equipCapacity;
    public Dictionary<PlayerStatType, float> AddStatus => addStatus;
    public SkillSlot DashSkill => dashSkill;
    public SkillSlot BasicAttack => basicAttackSlot;
    //delitgate
    public Action OnChangedActiveSkill;
    public Action OnChangedPassiveSkill;

    public void Init()
    {
        if (activeSkills != null && activeSkills.Count > 0) return;

        activeSkills = new List<SkillSlot>(activeCapacity);
        skillequip = new List<SkillSlot>(equipCapacity);
        passiveSkills = new List<PassiveSlot>(passiveCapacity);
        addStatus = new Dictionary<PlayerStatType, float>();
        dashSkill = new SkillSlot();
        basicAttackSlot.skill = basicAttack;
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
        OnChangedPassiveSkill += CalculatePassiveStat;
    }
    public bool AddPassiveSkill(PassiveSkill passiveSkill)
    {
        PassiveSlot emptySlot = passiveSkills.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.passiveSkill = passiveSkill;
            OnChangedPassiveSkill?.Invoke();
            
            GlobalEvents.Notify($"패시브 스킬 {passiveSkill.Data.Name}을 획득했습니다", 4f);
            return true;
        }
        return false;
    }
    public bool AddActiveSkill(Skill skill)
    {

        if (CheckDuplication(skill))
        {
            Debug.Log("중복스킬입니다");
            return false;
        }

        SkillSlot emptySlot = activeSkills.Find(s => s.IsEmpty());
        if (emptySlot != null)
        {
            emptySlot.skill = skill;
            OnChangedActiveSkill?.Invoke();
            GlobalEvents.Notify($"액티브 스킬 {skill.Data.SkillName}을 획득했습니다", 4f);
            return true;
        }

        Debug.Log("스킬창에 빈 슬롯이 없습니다");
        return false;
    }
    public bool AddDashSkill(Skill skill)
    {
        if (dashSkill.IsEmpty())
        {
            dashSkill.skill = skill;
            GlobalEvents.Notify($"대쉬 스킬을 획득했습니다", 4f);
            Debug.Log("대쉬스킬 획득");
            return true;
        }
        Debug.Log("이미있음");
        return false;
    }
    public bool CheckDashSkill()
    {
        if (dashSkill.IsEmpty())
        {
            return false;
        }
        return true;
    }
    public void Clear()
    {
        for (int i = 0; i < activeCapacity; i++)
        {
            activeSkills[i].Clear();
        }
        for (int i = 0; i < passiveCapacity; i++)
        {
            passiveSkills[i].Clear();
        }
        OnChangedActiveSkill?.Invoke();
        OnChangedPassiveSkill?.Invoke();
    }
    public void RegistActiveSkill(SkillSlot slot, int index)
    {
        if (slot.IsEmpty()) return;
        if (CheckDuplicationEquip(slot.skill))
        {
            Debug.Log("중복스킬등록 불가");
            return;
        }
        skillequip[index].Clear();
        skillequip[index].skill = slot.skill;
        OnChangedActiveSkill?.Invoke();
    }
    public bool CheckDuplication(Skill skill)
    {
        bool check = false;
        for (int i = 0; i < activeSkills.Count; i++)
        {
            if (activeSkills[i].skill == skill)
            {
                check = true;
                break;
            }
        }
        return check;
    }
    public bool CheckDuplicationEquip(Skill skill)
    {
        bool check = false;
        for (int i = 0; i < skillequip.Count; i++)
        {
            if (skillequip[i].skill == skill)
            {
                check = true;
                break;
            }
        }
        return check;
    }
    public void UnregistSkill(int index)
    {
        skillequip[index].Clear();
        OnChangedActiveSkill?.Invoke();
    }

    public void CalculatePassiveStat()
    {
        //Passive에만 스탯이 달려있으니 계산해서 넘겨주자
        addStatus.Clear();
        foreach (var s in passiveSkills)
        {
            if (s == null || s.IsEmpty() || s.passiveSkill == null) continue;
            foreach (var stat in s.passiveSkill.PassiveEffect)
            {
                if (addStatus.ContainsKey(stat.Type))
                {
                    addStatus[stat.Type] += stat.StatValue;
                }
                else
                {
                    addStatus.Add(stat.Type, stat.StatValue);
                }
            }
        }
        if (GameManager.instance.Player != null)
        {
            GameManager.instance.Player.StatusSystem.UpdateFinalStat();
        }

    }
    public void Save(SaveData data)
    {
        data.hasDashSkill = !dashSkill.IsEmpty();
        data.ownedActiveIDs.Clear();
        foreach (var slot in activeSkills)
        {
            if (!slot.IsEmpty()) data.ownedActiveIDs.Add(slot.skill.Data.SkillID);
        }

        data.ownedPassiveIDs.Clear();
        foreach (var slot in passiveSkills)
        {
            if (!slot.IsEmpty()) data.ownedPassiveIDs.Add(slot.passiveSkill.Data.PassiveID);
        }

        data.quickSlotIDs.Clear();
        foreach (var slot in skillequip)
        {
            data.quickSlotIDs.Add(!slot.IsEmpty() ? slot.skill.Data.SkillID : -1);
        }
    }

    // [LOAD]
    public void Load(SaveData data)
    {
        if (data.hasDashSkill)
        {
            dashSkill.skill = DataManager.instance.GetSkill(1000);
        }
        for (int i = 0; i < activeSkills.Count; i++)
        {
            if (i < data.ownedActiveIDs.Count)
            {
                activeSkills[i].skill = DataManager.instance.GetSkill(data.ownedActiveIDs[i]);
            }
            else activeSkills[i].Clear();
        }

        for (int i = 0; i < passiveSkills.Count; i++)
        {
            if (i < data.ownedPassiveIDs.Count)
            {
                passiveSkills[i].passiveSkill = DataManager.instance.GetPassive(data.ownedPassiveIDs[i]);
            }
            else passiveSkills[i].Clear();
        }

        for (int i = 0; i < skillequip.Count; i++)
        {
            if (i < data.quickSlotIDs.Count)
            {
                int id = data.quickSlotIDs[i];
                skillequip[i].skill = (id != -1) ? DataManager.instance.GetSkill(id) : null;
            }
        }

        OnChangedActiveSkill?.Invoke();
        OnChangedPassiveSkill?.Invoke(); 
    }
}
