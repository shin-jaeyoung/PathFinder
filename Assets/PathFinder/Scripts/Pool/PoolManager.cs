using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Monster,
    Skill,
    BossPattern,
    UI,
    DamageText,
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    [Header("Input PoolSO")]
    [SerializeField]
    private List<Pool> pools;

    //실제 쓰이는 정보
    private Dictionary<PoolType, Pool> poolDic = new Dictionary<PoolType, Pool>();

    [Header("PoolParent")]
    [SerializeField]
    private GameObject parentPrefab;
    private Dictionary<PoolType, GameObject> poolParentDic = new Dictionary<PoolType, GameObject>();

    //property

    public Dictionary<PoolType, Pool> PoolDic => poolDic;
    public Dictionary<PoolType,GameObject> PoolParentDic => poolParentDic;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach(var pool in pools)
        {
            if (pool == null) continue;
            if (!poolDic.ContainsKey(pool.type))
            {
                poolDic.Add(pool.type, pool);
                GameObject go = Instantiate(parentPrefab, transform);
                go.name = pool.type.ToString();
                poolParentDic.Add(pool.type, go);
            }
            pool.Init();
        }
    }
}

