using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterClass
{
    Normal,
    Elite,
    Boss,
}

[System.Serializable]
public struct MonsterData 
{
    [Header("Monster Info")]
    [SerializeField]
    private string mName;
    [SerializeField]
    private MonsterClass mClass;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float defence;
    [SerializeField]
    private float speed;

    public string Name => mName;
    public MonsterClass Class => mClass;
    public float HP => hp;
    public float MaxHp => maxHp;
    public float Atk => atk;
    public float Defence => defence;
    public float Speed => speed;

}
