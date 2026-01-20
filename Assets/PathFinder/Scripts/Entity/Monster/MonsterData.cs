using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
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
    private int id;
    [SerializeField]
    private string mName;
    [SerializeField]
    private MonsterType type;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float defence;
    [SerializeField]
    private float speed;

    public int Id => id;
    public string Name => mName;
    public MonsterType Type => type;
    public float MaxHp => maxHp;
    public float Atk => atk;
    public float Defence => defence;
    public float Speed => speed;

}
