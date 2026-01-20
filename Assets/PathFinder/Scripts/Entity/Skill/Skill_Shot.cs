using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Shot", menuName = "Skill/SkillType/Shot")]

public class Skill_Shot : Skill
{
    [SerializeField]
    private float shotSpeed;
    [SerializeField]
    private float count;
    [SerializeField]
    private float deltaShot;
    public override void Execute(ISkillActive caster)
    {
        //풀링
    }
    public void Shot(GameObject prefab)
    {
        if(prefab.TryGetComponent(out Rigidbody2D rb))
        {
            //시전자가 바라보는 곳 or 플레이어 경우 마우스 포인트방향으로 발사 => 이거 전체 적용해야 할듯
            //rb.transform.forward = 
        }
    }
}
