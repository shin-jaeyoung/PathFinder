using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillInventory : MonoBehaviour
{
    [Header("Skill Inventory")]
    [SerializeField]
    private List<string> skills;
    [SerializeField]
    private int capacity;

    private void Awake()
    {
        skills = new List<string>();
        for(int i = 0; i < capacity; i++)
        {
            skills.Add("1");
        }
    }

}
