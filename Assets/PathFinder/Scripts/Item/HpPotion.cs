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

    private Player player;
    //property
    public ItemData Data => data;
    public float HealMount => healMount;
    public int CurCount => curCount;
    public bool IsCoolTime => isCoolTime;

    //deligate
    public Action OnChanged;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        if (GameManager.instance.Player != null)
        {
            player = GameManager.instance.Player;
            maxCount = 5;
            curCount = maxCount;
            player.LevelSystem.OnLevelChanged += Upgrade;
            StartCoroutine(PotionGenerate());
        }
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.LevelSystem.OnLevelChanged -= Upgrade;
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
        player.StatusSystem.Heal(healMount * player.StatusSystem.FinalStat[PlayerStatType.MaxHp]);
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
        OnChanged?.Invoke();
    }
}
