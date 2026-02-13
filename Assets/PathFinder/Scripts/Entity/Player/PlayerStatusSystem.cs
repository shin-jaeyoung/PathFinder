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
    //추가스펙스텟을 더해준 스탯 (패시브스킬, 장비)
    private Dictionary<PlayerStatType, float> finalStat;
    //STR,DEX,CON을 기본능력치로 치환
    private Dictionary<PlayerStatType, float> substitution;
    private Player player;

    //property
    public Dictionary<PlayerStatType, float> Stat => stat;
    public Dictionary<PlayerStatType, float> FinalStat => finalStat;

    //deligate
    public Action OnStatChanged;
    public Action OnDead;


    public void Init(PlayerStatList initData)
    {
        stat = new Dictionary<PlayerStatType, float>();
        currenStats = new List<PlayerStats>();
        finalStat = new Dictionary<PlayerStatType, float>();
        substitution = new Dictionary<PlayerStatType, float>();
        player = GameManager.instance.Player;
        if (initData == null) return;
        initStats = initData;
        for (int i =0; i < initStats.StatsList.Count; i++)
        {
            stat.Add(initStats.StatsList[i].Type, initStats.StatsList[i].StatValue);
        }
        
        player.Skills.OnChangedPassiveSkill += UpdateFinalStat;
        player.Inventory.OnEquipValueChanged += UpdateFinalStat;
        OnStatChanged += UpdateStat;

        UpdateFinalStat();
    }
    public void UpdateStat()
    {
        currenStats.Clear();
        foreach (KeyValuePair<PlayerStatType, float> pair in finalStat)
        {
            if (pair.Key == PlayerStatType.CurHp)
            {
                currenStats.Add(new PlayerStats(pair.Key, stat[pair.Key]));
            }
            else 
            { 
                currenStats.Add(new PlayerStats(pair.Key, pair.Value)); 
            }
        }
    }
    public void AddStat(PlayerStatType type, int value)
    {
        if (stat.ContainsKey(type))
        {
            if( type == PlayerStatType.STR || type == PlayerStatType.DEX || type == PlayerStatType.CON)
            {
                if (!player.LevelSystem.UseLevelPoint()) return;
            }
            stat[type] += value;

            if (type == PlayerStatType.CurHp)
            {
                float maxHp = finalStat.ContainsKey(PlayerStatType.MaxHp) ?
                              finalStat[PlayerStatType.MaxHp] : stat[PlayerStatType.MaxHp];

                if (stat[type] > maxHp) stat[type] = maxHp;
            }
            UpdateFinalStat();
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
    public void UpdateFinalStat()
    {
        finalStat.Clear();
        foreach (var p in stat)
        {
            finalStat[p.Key] = p.Value;
        }

        AddStatsFromSource(player.Skills.AddStatus);
        AddStatsFromSource(player.Inventory.EquipmentStat);
        AddStatsFromSource(SubstitutionStat(finalStat));

        OnStatChanged?.Invoke();
    }
    //내부요인 말고 외부요인(장비,스킬)
    private void AddStatsFromSource(Dictionary<PlayerStatType, float> source)
    {
        if (source == null) return;

        foreach (var pair in source)
        {
            if (finalStat.ContainsKey(pair.Key))
                finalStat[pair.Key] += pair.Value;
            else
                finalStat[pair.Key] = pair.Value;
        }
    }
    private Dictionary<PlayerStatType, float> SubstitutionStat(Dictionary<PlayerStatType, float> source)
    {
        substitution.Clear();
        float str = source[PlayerStatType.STR]; // str 1당 power 1 maxhp10
        float dex = source[PlayerStatType.DEX]; // dex 1당 crirate 0.5 , cridmg 1
        float con = source[PlayerStatType.CON]; // con 1당 def 1 , maxhp 20

        substitution.Add(PlayerStatType.Power, str);
        substitution.Add(PlayerStatType.Armor, con);
        substitution.Add(PlayerStatType.CriRate, dex * 0.5f);
        substitution.Add(PlayerStatType.CriDamage, dex);
        substitution.Add(PlayerStatType.MaxHp, str * 10f + con * 20f);

        return substitution;
    }
    public void TakeDamage(float value)
    {
        ReduceStat(PlayerStatType.CurHp, (int)value);
    }
    
    public void Heal(float value)
    {
        AddStat(PlayerStatType.CurHp, (int)value);
    }
    public int FinalDamage()
    {
        int finalDamage = 0;
        float power = finalStat[PlayerStatType.Power];
        float criRate = finalStat[PlayerStatType.CriRate];
        float cridamage = finalStat[PlayerStatType.CriDamage];

        int randomNum = UnityEngine.Random.Range(0, 100);
        if( randomNum <criRate)
        {
            Debug.Log("크리티컬");
            power *= 1 + (cridamage + 40) / 100;
        }
        finalDamage = Mathf.RoundToInt(power);

        return finalDamage;
    }
}
