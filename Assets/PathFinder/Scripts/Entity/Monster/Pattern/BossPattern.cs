using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPattern : MonoBehaviour, IPoolable
{
    [Header("Pooldata_PatternID")]
    [SerializeField]
    private int id;
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetID()
    {
        return id;
    }
}
