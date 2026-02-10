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
    private float firstSpawnDelay;
    [SerializeField]
    private float shotDelay;
    [SerializeField]
    private float spawnDistance;
    [SerializeField]
    private bool canPass;
    [Header("Offset")]
    [SerializeField]
    private Vector3 offset;
    public override void Execute(ISkillActive caster)
    {
        caster.GetEntity().StartCoroutine(ShotDelayCo(caster));
    }
    private IEnumerator ShotDelayCo(ISkillActive caster)
    {
        WaitForSeconds wait = new WaitForSeconds(shotDelay);
        int curcount = count;
        yield return new WaitForSeconds(firstSpawnDelay);
        while (curcount > 0)
        {
            Vector2 dir = caster.LookDir();
            Vector2 origin = caster.CasterTrasform() + offset;

            Shot(caster,dir,origin);
            curcount--;
            yield return wait;
        }
    }

    private void Shot(ISkillActive caster, Vector2 dir, Vector2 spawnPos)
    {
        if (caster == null || caster.GetEntity() == null) return;

        spawnPos = spawnPos + dir * spawnDistance;


        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(data.ID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            pj.Init(caster.GetAttackPower() * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(), canPass);
            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pj.rb.rotation = angle + data.SpriteRotation;
            pj.rb.velocity = dir * shotSpeed;
            pj.StartCoroutine(SkillReturnCo(pj));
        }

    }
}
