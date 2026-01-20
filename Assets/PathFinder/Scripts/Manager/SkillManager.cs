using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;


    public Action OnCooltimeReduced;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UseSkill(ISkillActive caster, SkillSlot slot)
    {
        if (caster == null) return;
        if (slot.IsEmpty() || slot.isCooltime) return;
        slot.Use(caster);
        if(caster is  Player)
        {
            StartCoroutine(SkillCooltime(slot));
        }
    }

    private IEnumerator SkillCooltime(SkillSlot slot)
    {
        slot.isCooltime = true;
        float cooltime = slot.skill.Data.Cooltime;
        slot.currentCooltime = cooltime;
        while(slot.currentCooltime > 0)
        {
            if (slot.IsEmpty()) yield break;
            slot.currentCooltime -= Time.deltaTime;
            OnCooltimeReduced?.Invoke();
            yield return null;
        }
        slot.currentCooltime = 0;
        slot.isCooltime = false;
    }
}
