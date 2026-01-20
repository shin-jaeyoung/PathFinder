using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatSystem
{
    private Entity owner;

    public void Init(Entity owner)
    {
        this.owner = owner;
    }
    public void PerformSkill(SkillSlot slot)
    {
        if (slot.IsEmpty() || slot.isCooltime) return;
        SkillManager.instance.UseSkill(owner, slot);
    }

    public float Hit(float damage,float def)
    {
        float finalDamage;
        //데미지 계산식
        finalDamage = damage - def;

        if(finalDamage<=0)
        {
            //최소 데미지
            finalDamage = 5;
        }
        return finalDamage;
    }
}
