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
            while( curExp >= maxExp )
            {
                curExp -= maxExp;
                LevelUP();
            }
            GlobalEvents.Notify($"{mount}만큼 경험치 획득",1f);
            OnExpChanged?.Invoke();
        }
        return false;
    }
    public void LevelUP()
    {
        level++;
        levelPoint++;
        UpdateMaxExp();
        GlobalEvents.Notify($"레벨 업! {level}레벨 이 되었습니다", 2f);
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
