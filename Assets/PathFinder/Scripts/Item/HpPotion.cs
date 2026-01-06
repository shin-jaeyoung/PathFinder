using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion :MonoBehaviour ,IUseable
{
    [SerializeField]
    private ItemData data;

    // value는 %로 생각 
    [SerializeField]
    private float value = 0.2f;
    [SerializeField]
    private float generateTime;
    [SerializeField]
    private float coolTime;
    [SerializeField]
    private bool isCoolTime;

    private int curCount;
    private int maxCount;

    public Action OnChanged;

    public void Use()
    {
        if(curCount>0)
        {
            if (!isCoolTime)
            {
                curCount--;
                Effect();
                isCoolTime = true;
                OnChanged?.Invoke();
            }
            else
            {
                Debug.Log("포션쿨타임");
            }
        }
        else
        {
            Debug.Log("포션부족");
        }
    }
    public void Effect()
    {
        GameManager.instance.Player.StatusSystem.Heal(value * GameManager.instance.Player.StatusSystem.Stat[PlayerStatType.MaxHp]);
    }

    public IEnumerator PotionGenerate()
    {
        WaitForSeconds time = new WaitForSeconds(generateTime);
        while(curCount<maxCount)
        {
            yield return time;
            curCount++;
        }
    }
    public IEnumerator PotionCooltime()
    {
        while (isCoolTime)
        { 
            yield return new WaitForSeconds(coolTime);
            isCoolTime = false;
        }

    }
}
