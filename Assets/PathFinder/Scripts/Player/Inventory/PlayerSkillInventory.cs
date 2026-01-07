using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillInventory : MonoBehaviour
{
    [Header("Skill Inventory")]
    [SerializeField]
    private List<SkillSlot> skills;
    [SerializeField]
    private int capacity;

    private void Awake()
    {
        skills = new List<SkillSlot>();
        for(int i = 0; i < capacity; i++)
        {
            skills.Add(new SkillSlot());
        }
    }

    public bool AddSkill(Skill skill)
    {

        return true;
    }
}
