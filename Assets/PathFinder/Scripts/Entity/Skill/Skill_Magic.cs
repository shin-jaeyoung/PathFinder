using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Magic", menuName = "Skill/SkillType/Magic")]
public class Skill_Magic : Skill
{
    [Header("Spawn")]
    [SerializeField]
    private int spawnCount;
    [SerializeField]
    private float firstSpawnDelay;
    [SerializeField]
    private float spawnDelay;
    [Header("Explosion")]
    [SerializeField]
    private bool isExplosion;
    [SerializeField]
    private int explosionID;
    [SerializeField]
    private float damageMultiplier;
    public override void Execute(ISkillActive caster)
    {
        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster));
    }
    private IEnumerator SpawnDelayCo(ISkillActive caster)
    {
        WaitForSeconds delay = new WaitForSeconds(firstSpawnDelay);
        WaitForSeconds spawndelta = new WaitForSeconds(spawnDelay);

        yield return delay;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPos = caster.SkillSpawnPos();

            yield return spawndelta;

            Magic(caster, spawnPos, data.ProjecitileID);

        }
    }
    public void Magic(ISkillActive caster , Vector2 spawnPos , int originID)
    {
        if (caster == null || caster.GetEntity() == null) return;


        //풀링해야할곳
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(originID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            DamageInfo info = caster.GetDamageInfo();
            pj.Init(info.damage * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(), info.isCritical);
            if(isExplosion)
            {
                caster.GetEntity().StartCoroutine(ExplosionDelayCo(caster, spawnPos));
            }
            pj.StartCoroutine(SkillReturnCo(pj));
        }
    }
    private IEnumerator ExplosionDelayCo(ISkillActive caster, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(data.Duration);
        Explosion(caster, spawnPos, explosionID);
    }
    
    public void Explosion(ISkillActive caster,Vector2 spawnPos, int explosionID)
    {
        if (caster == null || caster.GetEntity() == null) return;

        

        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(explosionID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            DamageInfo info = caster.GetDamageInfo();
            pj.Init(info.damage * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(), info.isCritical);

            pj.StartCoroutine(SkillReturnCo(pj));
        }
    }
}
