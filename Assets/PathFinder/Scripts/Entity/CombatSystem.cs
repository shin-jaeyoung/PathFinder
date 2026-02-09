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
    public bool PerformSkill(SkillSlot slot)
    {
        if (slot.IsEmpty() || slot.isCooltime) return false;
        SkillManager.instance.UseSkill(owner, slot);
        return true;
    }

    public int Hit(float damage,float def)
    {
        float finalDamage;
        //데미지 계산식
        finalDamage = damage - def;

        if(finalDamage<=3)
        {
            //최소 데미지
            finalDamage = 3;
        }
        int finalDamageToint = Mathf.RoundToInt(finalDamage);


        return finalDamageToint;
    }
}
