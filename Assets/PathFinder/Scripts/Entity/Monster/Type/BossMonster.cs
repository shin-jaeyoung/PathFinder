using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{

    [Header("Gimmick Settings")]
    [SerializeField]
    private float gimmickInterval;
    private bool isGimmickActive = false;
    [Header("Pattern_FindZone")]
    [SerializeField]
    private float alertTime;
    [SerializeField]
    private int totalZones;
    [SerializeField]
    private GameObject safetyZonePrefab;
    [SerializeField]
    private GameObject damageZonePrefab;
    List<GameObject> spawnedZones = new List<GameObject>();
    //property
    public bool IsGimmickActive => isGimmickActive;
    
    protected override void InitStart()
    {
        //보스몹의 추가 상태
        stateMachine.AddState(StateType.Immortal, new MonsterImmortalState());

        StartCoroutine(GimmickTimerCo());
    }

    private IEnumerator GimmickTimerCo()
    {
        WaitForSeconds wait = new WaitForSeconds(gimmickInterval);
        while (true)
        {
            yield return wait;

            if (curHp > 0 && !isGimmickActive)
            {
                GimmickStart();
            }
        }
    }

    public void GimmickStart()
    {
        Debug.Log("기믹시작");
        isGimmickActive = true;
        stateMachine.ChangeState(StateType.Immortal);
        StartCoroutine(SelectRandomGimmickCo());
    }
    public void GimmickEnd()
    {
        isGimmickActive = false;
        stateMachine.ChangeState(StateType.Move);
    }
    private IEnumerator SelectRandomGimmickCo()
    {
        
        yield return StartCoroutine(Pattern_ZoneMixture());

        GimmickEnd();
    }

    //패턴매서드
    private IEnumerator Pattern_ZoneMixture()
    {
        Debug.Log("기믹 발동: 진짜 안전지대를 찾아라ㅋ");

        
        int safeIndex = Random.Range(0, totalZones); 

        for (int i = 0; i < totalZones; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 6f;

            GameObject prefab = (i == safeIndex) ? safetyZonePrefab : damageZonePrefab;
            GameObject zone = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedZones.Add(zone);

        }
        float tmp = alertTime;

        while (alertTime > 0)
        {
            alertTime -= Time.deltaTime;
            yield return null;
        }
        alertTime = tmp;
        ExecuteUltimateAttack();

        // 4. 장판 정리
        foreach (var zone in spawnedZones) Destroy(zone);
        yield return new WaitForSeconds(1f);
    }

    private void ExecuteUltimateAttack()
    {

        if (GameManager.instance.Player.IsInvincible == false)
        {
            GameManager.instance.Player.Hit(new DamageInfo(100000,this,GameManager.instance.Player));
        }
        else
        {
            Debug.Log("플레이어가 안전지대에서 생존했습니다.");
        }
    }


}
