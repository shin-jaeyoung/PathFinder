using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePool<T> : Pool where T : Component,IPoolable
{
    [SerializeField] protected List<T> prefabs;

    protected Dictionary<int, T> prefabDic = new Dictionary<int, T>();
    protected Dictionary<int, Queue<GameObject>> poolDic = new Dictionary<int, Queue<GameObject>>();

    public override void Init()
    {
        prefabDic.Clear();
        poolDic.Clear();

        foreach (var item in prefabs)
        {
            if (item == null) continue;
            int id = item.GetID();

            if (!prefabDic.ContainsKey(id))
            {
                prefabDic.Add(id, item);
                poolDic.Add(id, new Queue<GameObject>());

                
            }
            else
            {
                // 이 로그가 찍힌다면 인스펙터에서 ID 설정을 잘못하신 겁니다!
                Debug.LogError($"{id}번 ID가 중복되었습니다! 중복 프리팹: {item.name}");
            }
        }
    }

    private GameObject CreateNewObject(int id)
    {
        GameObject obj = Instantiate(prefabDic[id].gameObject, PoolManager.instance.PoolParentDic[type].transform);
        obj.SetActive(false);
        return obj;
    }

    public override GameObject Pop(int id, Vector2 position, Quaternion rotation)
    {
        if (!prefabDic.ContainsKey(id)) return null;

        GameObject obj = (poolDic[id].Count > 0) ? poolDic[id].Dequeue() :
                         CreateNewObject(id);

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public override void ReturnPool(IPoolable obj)
    {

        int id = obj.GetID();
        GameObject go = obj.GetGameObject();
        if (poolDic.ContainsKey(id))
        {
            go.SetActive(false);
            go.transform.SetParent(PoolManager.instance.PoolParentDic[type].transform);
            poolDic[id].Enqueue(go);
        }
        else
        {
            Destroy(go);
        }
        
    }
}
