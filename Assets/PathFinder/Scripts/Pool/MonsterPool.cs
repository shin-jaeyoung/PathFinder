using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName ="MonsterPool",menuName = "Pool/Monster")]
public class MonsterPool : Pool
{
    [SerializeField]
    private List<Monster> monsters;

    private Dictionary<int, Monster> prefabDic = new Dictionary<int, Monster>();
    private Dictionary<int,Queue<GameObject>> poolDic = new Dictionary<int, Queue<GameObject>>();
    public override void Init()
    {
        prefabDic.Clear();
        poolDic.Clear();

        foreach (var mon in monsters)
        {
            if (mon == null) continue;
            int id = mon.Data.Id;

            if (!prefabDic.ContainsKey(id))
            {
                prefabDic.Add(id, mon);
                poolDic.Add(id, new Queue<GameObject>());
                GameObject obj = Instantiate(prefabDic[id].gameObject, PoolManager.instance.PoolParentDic[type].transform);
                obj.SetActive(false);
                poolDic[id].Enqueue(obj);
            }
            else
            {
                Debug.Log($"{id}번 몬스터 ID 중복!");
            }
        }
    }

    public override GameObject Pop(int id, Vector2 position, Quaternion rotation)
    {
        if (!prefabDic.ContainsKey(id))
        {
            Debug.Log("해당 몬스터 MonsterPool에 등록해");
            return null;
        }
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
            obj = Instantiate(prefabDic[id].gameObject, position, rotation, PoolManager.instance.PoolParentDic[type].transform);
        }

        return obj;
    }
    public override void ReturnPool(GameObject obj)
    {
        if(obj.TryGetComponent<IPoolable>(out IPoolable poolable))
        {
            int id = poolable.GetID();

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
