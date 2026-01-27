using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPool : Pool
{
    [SerializeField]
    private List<Monster> monsters;
    private Dictionary<int, Monster> monsterDic = new Dictionary<int, Monster>();

    public override void Init()
    {
        if (monsters.Count < 1) return;
        foreach (var mon in monsters)
        {
            if (!monsterDic.ContainsKey(mon.Data.Id))
            {
                monsterDic.Add(mon.Data.Id,mon);
            }
            else
            {
                Debug.Log("몬스터 아이디중복, 체크좀 해줘");
            }
        }
    }

    public override GameObject ID2GameObj(int id)
    {
        if (!monsterDic.ContainsKey(id))
        {
            Debug.Log("리스트에 없는 아이디");
            return null;
        }
        return monsterDic[id].gameObject;
    }
}
