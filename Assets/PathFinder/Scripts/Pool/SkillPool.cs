using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPool : Pool
{
    [SerializeField]
    private List<Skill> skills;

    private Dictionary<int, Skill> skillDic = new Dictionary<int, Skill>();

    public override void Init()
    {
        if (skills.Count < 1) return;
        foreach (var skill in skills)
        {
            if(!skillDic.ContainsKey(skill.Data.ID))
            {
                skillDic.Add(skill.Data.ID, skill);
            }
            else
            {
                Debug.Log("스킬 아이디중복, 체크좀 해줘");
            }
        }
    }
    public override GameObject ID2GameObj(int id)
    {
        if(!skillDic.ContainsKey(id))
        {
            Debug.Log("리스트에 없는 아이디");
            return null;
        }
        return skillDic[id].Data.Prefab;
    }
}
