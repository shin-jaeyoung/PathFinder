using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Magic", menuName = "Skill/SkillType/Magic")]
public class Skill_Magic : Skill
{
    [SerializeField]
    private float spawnDelay;
    [Header("Explosion")]
    [SerializeField]
    private bool isExplosion;
    [SerializeField]
    private GameObject explosionEffectPrefab;
    [SerializeField]
    private float damageMultiplier;
    public override void Execute(ISkillActive caster)
    {
        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster));
    }
    private IEnumerator SpawnDelayCo(ISkillActive caster)
    {
        yield return new WaitForSeconds(spawnDelay);
        Magic(caster);
    }
    public void Magic(ISkillActive caster)
    {
        if (caster == null || caster.GetEntity() == null) return;

        Vector2 spawnPos = caster.SkillSpawnPos();

        //풀링해야할곳
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(data.ID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(), this);
            if(isExplosion)
            {
                pj.StartCoroutine(ExplosionDelayCo(caster));
            }
            pj.StartCoroutine(SkillReturnCo(go));
        }
    }
    private IEnumerator ExplosionDelayCo(ISkillActive caster)
    {
        yield return new WaitForSeconds(data.Duration);
        Explosion(caster);
    }
    //이거는 풀링 못했음 ㅋㅋ
    public void Explosion(ISkillActive caster)
    {
        if (caster == null || caster.GetEntity() == null) return;

        Vector2 spawnPos = caster.SkillSpawnPos();

        GameObject go = Instantiate(explosionEffectPrefab, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * damageMultiplier, caster.GetEntity(), caster.GetEntityType(),this);

            pj.StartCoroutine(SkillDestroyCo(go));
        }
    }
}
