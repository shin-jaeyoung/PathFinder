using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelSystem 
{
    private int level = 1;
    private int curExp = 0;
    private int maxExp = 100;
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
            OnExpChanged?.Invoke();
        }
        return false;
    }
    public void LevelUP()
    {
        level++;
        levelPoint++;
        UpdateMaxExp();
        OnLevelChanged?.Invoke();
    }
}
