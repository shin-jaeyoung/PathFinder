using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Monster,
    Skill,

}
public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private MonsterPool monsterPool;
    [SerializeField]
    private SkillPool skillPool;

    private Dictionary<PoolType, Pool> poolDic = new Dictionary<PoolType, Pool>();

    private void Awake()
    {
        monsterPool.Init();
        skillPool.Init();
    }
}

[System.Serializable]
public abstract class Pool 
{
    public PoolType type;

    public abstract void Init();
    public abstract GameObject ID2GameObj(int id);
}