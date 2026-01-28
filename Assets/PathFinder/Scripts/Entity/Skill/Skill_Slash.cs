using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slash", menuName = "Skill/SkillType/Slash")]
public class Skill_Slash : Skill
{
    [SerializeField]
    private float spawnDistance;
    [SerializeField]
    private float spawnDelay;
    public override void Execute(ISkillActive caster)
    {
        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster));
    }
    public IEnumerator SpawnDelayCo(ISkillActive caster)
    {
        yield return new WaitForSeconds(spawnDelay);
        Slash(caster);
    }

    public void Slash(ISkillActive caster)
    {
        if (caster == null || caster.GetEntity() == null) return;
        Vector2 dir = caster.LookDir();
        Vector2 spawnPos = caster.CasterTrasform();
        spawnPos = spawnPos + dir * spawnDistance;


        //풀링하자 나중에
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(data.ID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(),this);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pj.rb.rotation = angle + data.SpriteRotation;

            pj.StartCoroutine(SkillReturnCo(go));
        }
    }
}
