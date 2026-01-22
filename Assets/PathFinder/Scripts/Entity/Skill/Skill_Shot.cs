using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Shot", menuName = "Skill/SkillType/Shot")]

public class Skill_Shot : Skill
{
    [SerializeField]
    private float shotSpeed;
    [SerializeField]
    private int count;
    [SerializeField]
    private float shotDelay;
    [SerializeField]
    private float spawnDistance;
    public override void Execute(ISkillActive caster)
    {
        //코루틴을 컴포넌트만 쓸수있어서
        SkillManager.instance.StartCoroutine(ShotDelayCo(caster));
    }
    private IEnumerator ShotDelayCo(ISkillActive caster)
    {
        WaitForSeconds wait = new WaitForSeconds(shotDelay);
        int curcount = count;
        while (curcount > 0)
        {
            Shot(caster);
            curcount--;
            yield return wait;
        }
    }
    
    private void Shot(ISkillActive caster)
    {
        if (caster == null || caster.GetEntity() == null) return;
        Vector2 dir = caster.LookDir();
        Vector2 spawnPos = caster.CasterTrasform();
        spawnPos = spawnPos + dir * spawnDistance;

        //나중에 풀링으로 바꾸기
        //테스트용

        GameObject go = Instantiate(data.Prefab, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(),true);
            Debug.Log(caster.GetAttackPower());
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pj.rb.rotation = angle + data.SpriteRotation;
            pj.rb.velocity = dir * shotSpeed;
            pj.StartCoroutine(SkillReturnCo(go));
        }

    }
}
