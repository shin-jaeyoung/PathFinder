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
    [Header("Multiple Slash Settings")]
    [SerializeField] 
    private List<int> otherSlashIDs;
    [SerializeField]
    private int spawnCount;
    [SerializeField]
    private float spawnDelta;
    [Header("Offset")]
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private bool isMonster;
    public override void Execute(ISkillActive caster)
    {

        caster.GetEntity().StartCoroutine(SpawnDelayCo(caster));
    }
    private IEnumerator SpawnDeltaCo(ISkillActive caster)
    {
        WaitForSeconds wait = new WaitForSeconds(spawnDelta);
        for (int i = 0; i < spawnCount; i++)
        {
            // 0번째는 기본 데이터 사용, 1번째부터는 리스트에서 순차적 사용
            int currentID = (i == 0 || otherSlashIDs.Count == 0)
                            ? data.ProjecitileID
                            : otherSlashIDs[(i - 1) % otherSlashIDs.Count];
            Vector2 dir = caster.LookDir();
            Vector2 spawnPos = caster.CasterTrasform()+offset;
            if (!isMonster)
            {
                Slash(caster, dir, spawnPos, currentID);
            }
            yield return wait;
            if (isMonster)
            {
                Slash(caster, dir, spawnPos, currentID);
            }
        }
    }
    public IEnumerator SpawnDelayCo(ISkillActive caster)
    {
        yield return new WaitForSeconds(spawnDelay);
        caster.GetEntity().StartCoroutine(SpawnDeltaCo(caster));
    }

    public void Slash(ISkillActive caster,Vector2 dir, Vector2 spawnPos, int projectileID)
    {
        if (caster == null || caster.GetEntity() == null) return;

        spawnPos = spawnPos + dir * spawnDistance;


        //풀링하자 나중에
        GameObject go = PoolManager.instance.PoolDic[PoolType.Skill].Pop(projectileID, spawnPos, Quaternion.identity);
        if (go.TryGetComponent(out Projectile pj))
        {
            DamageInfo info = caster.GetDamageInfo();
            pj.Init(info.damage * data.DamageMultiplier, caster.GetEntity(), caster.GetEntityType(), info.isCritical);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            pj.rb.rotation = angle + data.SpriteRotation;

            pj.StartCoroutine(SkillReturnCo(pj));
        }
    }
}
