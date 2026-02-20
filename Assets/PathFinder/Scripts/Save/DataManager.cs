using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    // ID를 키값으로 하는 데이터베이스 딕셔너리
    private Dictionary<int, Item> itemDB = new Dictionary<int, Item>();
    private Dictionary<int, Skill> skillDB = new Dictionary<int, Skill>();
    private Dictionary<int, PassiveSkill> passiveDB = new Dictionary<int, PassiveSkill>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllDatabases();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void LoadAllDatabases()
    {
        // 아이템 로드 (Resources/Items 폴더 기준)
        Item[] items = Resources.LoadAll<Item>("Items");
        foreach (var item in items)
        {
            if (!itemDB.ContainsKey(item.Data.ID))
                itemDB.Add(item.Data.ID, item);
        }

        // 액티브 스킬 로드 (Resources/Skills 폴더 기준)
        Skill[] skills = Resources.LoadAll<Skill>("Skills");
        foreach (var skill in skills)
        {
            if (!skillDB.ContainsKey(skill.Data.SkillID))
                skillDB.Add(skill.Data.SkillID, skill);
        }

        // 패시브 스킬 로드 (Resources/Passives 폴더 기준)
        PassiveSkill[] passives = Resources.LoadAll<PassiveSkill>("Passives");
        foreach (var p in passives)
        {
            if (!passiveDB.ContainsKey(p.Data.PassiveID))
                passiveDB.Add(p.Data.PassiveID, p);
        }

        Debug.Log($"로드 완료: 아이템({itemDB.Count}), 스킬({skillDB.Count}), 패시브({passiveDB.Count})");
    }



    public Item GetItem(int id)
    {
        if (id == -1) return null;
        if (itemDB.TryGetValue(id, out Item item)) return item;

        Debug.LogWarning($"ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
        return null;
    }

    public Skill GetSkill(int id)
    {
        if (id == -1) return null;
        if (skillDB.TryGetValue(id, out Skill skill)) return skill;

        Debug.LogWarning($"ID {id}에 해당하는 스킬을 찾을 수 없습니다.");
        return null;
    }

    public PassiveSkill GetPassive(int id)
    {
        if (id == -1) return null;
        if (passiveDB.TryGetValue(id, out PassiveSkill passive)) return passive;

        Debug.LogWarning($"[DataManager] ID {id}에 해당하는 패시브를 찾을 수 없습니다.");
        return null;
    }
}