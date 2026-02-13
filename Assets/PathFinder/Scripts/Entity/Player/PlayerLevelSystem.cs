using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLevelSystem 
{
    [Header("Level")]
    [SerializeField]
    private int level;
    [SerializeField]
    private int maxLevel;
    [SerializeField]
    private int curExp;
    [SerializeField]
    private int maxExp;
    [SerializeField]
    private int levelPoint;
    [Header("Level Skill List")]
    [SerializeField]
    private List<Skill> levelSkillList;
    public int Level => level;
    public int CurExp => curExp;
    public int MaxExp => maxExp;
    public int LevelPoint => levelPoint;

    public event Action OnExpChanged;
    public event Action OnLevelChanged;
    public event Action OnLevelPointChanged;
    public void Init()
    {
        level = 1;
        curExp = 0;
        maxExp = 0;
        levelPoint = 0;
        UpdateMaxExp();
    }
    public void UpdateMaxExp()
    {
        maxExp = level * 100;
    }
    public bool AddExp(int mount)
    {
        if( mount >=0)
        {
            curExp += mount;
            GlobalEvents.Notify($"{mount}만큼 경험치 획득",2f);
            while( curExp >= maxExp )
            {
                curExp -= maxExp;
                LevelUP();
            }
            OnExpChanged?.Invoke();
        }
        return false;
    }
    public void LevelUP()
    {
        if(level < maxLevel)
        {
            level++;
            
            GlobalEvents.Notify($"레벨 업! {level}레벨 이 되었습니다", 2f);
            if( level == 5)
            {
                GameManager.instance.Player.Skills.AddActiveSkill(levelSkillList[0]);
            }
            else if (level == 10)
            {
                Debug.Log("10레벨 달성보상");
                GameManager.instance.Player.Skills.AddActiveSkill(levelSkillList[1]);
            }
            else if (level == 15)
            {
                Debug.Log("15레벨 달성보상");
                GameManager.instance.Player.Skills.AddActiveSkill(levelSkillList[2]);
            }
            levelPoint += 2;
            if (level == maxLevel) 
            {
                maxExp = int.MaxValue;
            }
            else 
            { 
                UpdateMaxExp(); 
            }
        }
        
        OnLevelChanged?.Invoke();
        OnLevelPointChanged?.Invoke();
    }

    public bool UseLevelPoint()
    {
        if(levelPoint>0)
        {
            levelPoint--;
            OnLevelPointChanged?.Invoke();
            return true;
        }
        else
        {
            Debug.Log("LevelPoint가 부족");
            return false;
        }
    }
}
