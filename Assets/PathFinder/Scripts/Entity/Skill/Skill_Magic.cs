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
        Vector2 spawnPos = caster.SkillSpawnPos();

        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster, spawnPos));
    }
    private IEnumerator SpawnDelayCo(ISkillActive caster, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(spawnDelay);
        Magic(caster, spawnPos);
    }
    public void Magic(ISkillActive caster , Vector2 spawnPos)
    {
        if (caster == null || caster.GetEntity() == null) return;


        //풀링해야할곳
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(data.ID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType());
            if(isExplosion)
            {
                pj.StartCoroutine(ExplosionDelayCo(caster, spawnPos));
            }
            pj.StartCoroutine(SkillReturnCo(pj));
        }
    }
    private IEnumerator ExplosionDelayCo(ISkillActive caster, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(data.Duration);
        Explosion(caster, spawnPos);
    }
    //이거는 풀링 못했음 ㅋㅋ
    public void Explosion(ISkillActive caster,Vector2 spawnPos)
    {
        if (caster == null || caster.GetEntity() == null) return;

        

        GameObject go = Instantiate(explosionEffectPrefab, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * damageMultiplier, caster.GetEntity(), caster.GetEntityType());

            pj.StartCoroutine(SkillDestroyCo(go));
        }
    }
}
