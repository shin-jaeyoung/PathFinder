using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : MonoBehaviour, IUseable
{
    [SerializeField]
    private ItemData data;

    [Header("Initial")]
    [SerializeField]
    float initialHealMount = 0.2f;
    [Header("Real")]
    [SerializeField]
    private float healMount = 0.2f;
    [SerializeField]
    private float generateTime;
    [SerializeField]
    private float coolTime;
    [SerializeField]
    private bool isCoolTime;

    [SerializeField]
    private int curCount;
    private int maxCount;

    //property
    public ItemData Data => data;
    public float HealMount => healMount;
    public int CurCount => curCount;
    public bool IsCoolTime => isCoolTime;

    //deligate
    public Action OnChanged;

    private void Start()
    {
        maxCount = 5;
        curCount = maxCount;
        StartCoroutine(PotionGenerate());
    }
    private void OnEnable()
    {
        if (GameManager.instance!= null && GameManager.instance.Player != null)
        {
            GameManager.instance.Player.LevelSystem.OnLevelChanged += Upgrade;
        }
    }
    private void OnDisable()
    {
        if (GameManager.instance!= null && GameManager.instance.Player != null)
        {
            GameManager.instance.Player.LevelSystem.OnLevelChanged -= Upgrade;
        }
    }
    public void Use()
    {
        if (curCount > 0 && !isCoolTime)
        {
            curCount--;
            Effect();
            isCoolTime = true;
            StartCoroutine(PotionCooltime());
            OnChanged?.Invoke();
        }
        else
        {
            Debug.Log(isCoolTime? "포션쿨타임" : "포션부족");
        }
    }
    public void Effect()
    {
        GameManager.instance.Player.StatusSystem.Heal(healMount * GameManager.instance.Player.StatusSystem.Stat[PlayerStatType.MaxHp]);
    }
    public void Upgrade()
    {
        float addValue = 0.01f * GameManager.instance.Player.LevelSystem.Level;
        healMount = initialHealMount + addValue;
    }
    public IEnumerator PotionGenerate()
    {
        WaitForSeconds time = new WaitForSeconds(generateTime);
        while (true)
        {
            if (curCount < maxCount)
            {
                yield return time;
                curCount++;
                OnChanged?.Invoke();
            }
            else
            {
                yield return null;
            }
        }

    }
    public IEnumerator PotionCooltime()
    {
        yield return new WaitForSeconds(coolTime);
        isCoolTime = false;
    }
}
