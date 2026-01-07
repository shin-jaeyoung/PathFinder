using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [SerializeField]
    private PlayerStatType type;
    [SerializeField]
    private float statValue;

    public PlayerStats(PlayerStatType type, float statValue)
    {
        this.type = type;
        this.statValue = statValue;
    }
    //property
    public PlayerStatType Type => type;
    public float StatValue => statValue;
}

[System.Serializable]
public class PlayerStatusSystem 
{
    //초기설정 스텟
    private PlayerStatList initStats;
    //스펙을 인스펙터에서 보기 위한 리스트 (이값은 딕셔너리를 복사한것)
    [SerializeField]
    private List<PlayerStats> currenStats;
    //실제 사용될 내부 스텟
    private Dictionary<PlayerStatType, float> stat;

    //property
    public Dictionary<PlayerStatType, float> Stat => stat;

    //deligate
    public Action OnStatChanged;
    public Action OnDead;

 

    public void Init(PlayerStatList initData)
    {
        stat = new Dictionary<PlayerStatType, float>();
        currenStats = new List<PlayerStats>();
        if (initData == null) return;
        initStats = initData;
        for (int i =0; i < initStats.StatsList.Count; i++)
        {
            stat.Add(initStats.StatsList[i].Type, initStats.StatsList[i].StatValue);
        }
        UpdateStat();
        OnStatChanged += UpdateStat;
    }
    public void UpdateStat()
    {
        currenStats.Clear();
        foreach (KeyValuePair<PlayerStatType, float> pair in stat)
        {
            currenStats.Add(new PlayerStats(pair.Key, pair.Value));
        }
    }
    public void AddStat(PlayerStatType type, int value)
    {
        if(stat.ContainsKey(type))
        {
            stat[type] += value;
            if (type == PlayerStatType.CurHp)
            {
                float maxHp = stat[PlayerStatType.MaxHp];
                if (stat[type] > maxHp)
                {
                    stat[type] = maxHp;
                }
            }
            OnStatChanged?.Invoke();
        }
    }
    public void ReduceStat(PlayerStatType type , int value)
    {
        if (stat.ContainsKey(type))
        {
            stat[type] -= value;
            if(stat[type] <= 0)
            {
                stat[type] = 0;
                if (type == PlayerStatType.CurHp)
                {
                    OnDead?.Invoke();
                }
            }

            OnStatChanged?.Invoke();
        }
    }
    public void TakeDamage(float value)
    {
        ReduceStat(PlayerStatType.CurHp, (int)value);
    }
    
    public void Heal(float value)
    {
        AddStat(PlayerStatType.CurHp, (int)value);
    }
}
