using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUI : MonoBehaviour
{
    [Header("Name")]
    [SerializeField]
    private TextMeshProUGUI nameText;
    [Header("Hp")]
    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private TextMeshProUGUI hpText;

    private Monster targetMonster;
    private void Awake()
    {
        gameObject.SetActive(false);
        GlobalEvents.OnEncountBoss += ShowHpUI;
    }


    public void ShowHpUI(Monster monster)
    {
        if (monster == null) 
        {
            CloseUI();
            return;
        }

        targetMonster = monster;
        gameObject.SetActive(true);

        targetMonster.OnChangeHp -= UpdateHpUI;
        targetMonster.OnChangeHp += UpdateHpUI;

        UpdateHpUI();
    }

    public void UpdateHpUI()
    {
        if (targetMonster == null) return;

        nameText.text = targetMonster.Data.Name;
        hpImage.fillAmount = targetMonster.CurHp / targetMonster.Data.MaxHp;
        hpText.text = $"{Mathf.RoundToInt(targetMonster.CurHp)} / {targetMonster.Data.MaxHp}";

        if (targetMonster.CurHp <= 0)
        {
            CloseUI();
        }

    }

    private void CloseUI()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (targetMonster != null)
        {
            targetMonster.OnChangeHp -= UpdateHpUI;
            targetMonster = null;
        }
    }
}
