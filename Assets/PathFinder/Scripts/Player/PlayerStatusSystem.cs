using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusSystem
{
    private Dictionary<PlayerStatType, float> stat;
    public Dictionary<PlayerStatType, float> Stat => stat;

    public Action OnStatChanged;

    public PlayerStatusSystem()
    {
        stat = new Dictionary<PlayerStatType, float>();
        Init();
    }

    public void Init()
    {
        stat.Add(PlayerStatType.STR, 0);
        stat.Add(PlayerStatType.DEX, 0);
        stat.Add(PlayerStatType.CON, 0);
        stat.Add(PlayerStatType.Power, 10);
        stat.Add(PlayerStatType.CriRate, 5);
        stat.Add(PlayerStatType.CriDamage, 20);
        stat.Add(PlayerStatType.Armor, 0);
        stat.Add(PlayerStatType.SPEED, 2.5f);
        stat.Add(PlayerStatType.MaxHp, 100);
    }
    public void AddStat(PlayerStatType type, int value)
    {
        if(stat.ContainsKey(type))
        {
            stat[type] += value;
            OnStatChanged?.Invoke();
        }
    }
    public void Reduce(PlayerStatType type , int value)
    {
        if (stat.ContainsKey(type))
        {
            stat[type] -= value;
            if(stat[type] < 0)
            {
                stat[type] = 0;
            }
            OnStatChanged?.Invoke();
        }
    }
}
