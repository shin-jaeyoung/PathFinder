using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "Skill/SkillType/BasicAttack")]
public class Skill_PlayerBasicAttack : Skill
{
    [SerializeField]
    private float spawnDistance;
    [SerializeField]
    private float spawnDelay;
    public override void Execute(ISkillActive caster)
    {
        Vector2 dir = caster.LookDir();
        Vector2 spawnPos = caster.CasterTrasform();
        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster, dir, spawnPos));
    }
    public IEnumerator SpawnDelayCo(ISkillActive caster, Vector2 dir, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(spawnDelay);
        Slash(caster, dir, spawnPos);
    }

    public void Slash(ISkillActive caster, Vector2 dir, Vector2 spawnPos)
    {
        if (caster == null || caster.GetEntity() == null) return;

        spawnPos = spawnPos + dir * spawnDistance;


        //풀링하자 나중에
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(data.ID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType());
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pj.rb.rotation = angle + data.SpriteRotation;

            pj.StartCoroutine(SkillReturnCo(pj));
        }
    }
}
