using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (fileName ="SkillPool",menuName ="Pool/Skill")]
public class SkillPool : Pool
{
    [SerializeField]
    private List<Skill> skills;

    private Dictionary<int, Skill> prefabDic = new Dictionary<int, Skill>();
    private Dictionary<int, Queue<GameObject>> poolDic = new Dictionary<int, Queue<GameObject>>();

    public override void Init()
    {
        prefabDic.Clear();
        poolDic.Clear();

        foreach (var skill in skills)
        {
            if (skill == null) continue;
            int id = skill.Data.ID;

            if (!prefabDic.ContainsKey(id))
            {
                prefabDic.Add(id, skill);
                poolDic.Add(id, new Queue<GameObject>());
                GameObject obj = Instantiate(prefabDic[id].Data.Prefab,PoolManager.instance.PoolParentDic[type].transform);
                obj.SetActive(false);
                poolDic[id].Enqueue(obj);
            }
            else
            {
                Debug.Log($"{id}번 스킬 ID 중복");
            }
        }
    }

    public override GameObject Pop(int id, Vector2 position, Quaternion rotation)
    {
        if (!prefabDic.ContainsKey(id)) return null;

        GameObject obj = null;

        if (poolDic[id].Count > 0)
        {
            obj = poolDic[id].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.transform.SetParent(PoolManager.instance.PoolParentDic[type].transform);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefabDic[id].Data.Prefab, position, rotation, PoolManager.instance.PoolParentDic[type].transform);
        }

        return obj;
    }
    public override void ReturnPool(GameObject obj)
    {
        if(obj.TryGetComponent<Projectile>(out Projectile pj))
        {
            int id = pj.owner.GetID();

            if (poolDic.ContainsKey(id))
            {
                obj.SetActive(false);
                obj.transform.SetParent(PoolManager.instance.PoolParentDic[type].transform);
                poolDic[id].Enqueue(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }
}
