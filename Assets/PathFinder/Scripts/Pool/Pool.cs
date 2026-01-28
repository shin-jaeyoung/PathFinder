using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public abstract class Pool : ScriptableObject
{
    public PoolType type;

    public abstract void Init();
    public abstract GameObject Pop(int id, Vector2 position, Quaternion rotation);
    public abstract void ReturnPool(GameObject obj);
}
