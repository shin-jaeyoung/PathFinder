using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLevelSystem 
{
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int curExp = 0;
    [SerializeField]
    private int maxExp = 100;
    [SerializeField]
    private int levelPoint = 0;
    [Header("Level Skill List")]
    [SerializeField]
    private List<Skill> levelSkillList;
    public int Level => level;
    public int CurExp => curExp;
    public int MaxExp => maxExp;
    public int LevelPoint => levelPoint;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    public void Init()
    {
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
        UpdateMaxExp();
        OnLevelChanged?.Invoke();
    }

    public bool UseLevelPoint()
    {
        if(levelPoint>0)
        {
            levelPoint--;
            return true;
        }
        else
        {
            Debug.Log("LevelPoint가 부족");
            return false;
        }
    }
}
