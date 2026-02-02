using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossMonster : Monster
{

    [Header("Gimmick Settings")]
    [SerializeField]
    private Vector2 bossPosduringGimmick;
    [SerializeField]
    private float gimmickInterval;
    private bool isGimmickActive = false;
    [Header("Pattern_FindSafetyZone")]
    [SerializeField]
    private float alertTime;
    [SerializeField]
    private int totalZones;
    [SerializeField]
    private float spawnZoneDistance;
    [SerializeField]
    private int safetyZoneID;
    [SerializeField]
    private int damageZoneID;
    List<BossPattern> spawnedZones = new List<BossPattern>();

    [Header("Pattern_Totem")]
    [SerializeField]
    private int totemID;
    [SerializeField]
    private float spawnTotemDistance;
    [SerializeField]
    private float alertTime_perTotems;
    [SerializeField]
    private int aliveTotemCount;
    private List<Totem> totemSpawnList = new List<Totem>();


    //property
    public bool IsGimmickActive => isGimmickActive;
    
    protected override void InitStart()
    {
        //보스몹의 추가 상태
        //Immortal은 무적,고정용 상태임
        stateMachine.AddState(StateType.Immortal, new MonsterImmortalState());

        //기믹쿨타임용 코루틴
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
                yield return StartCoroutine(GimmickStart());
            }
        }
    }

    public IEnumerator GimmickStart()
    {
        Debug.Log("기믹시작");
        isGimmickActive = true;
        transform.position = bossPosduringGimmick;
        stateMachine.ChangeState(StateType.Immortal);
        yield return StartCoroutine(SelectRandomGimmickCo());
        GimmickEnd();
    }
    public void GimmickEnd()
    {
        isGimmickActive = false;
        stateMachine.ChangeState(StateType.Move);
        //여기에 플레이어에게 기믹이 끝났음을 알리는 효과가 있으면 좋을듯
    }
    
    private IEnumerator SelectRandomGimmickCo()
    {
        int randomChoice = Random.Range(0, 2);
        //토템 테스트용
        if(randomChoice == 0)
        {
            yield return StartCoroutine(Pattern_ZoneMixture());
        }
        else
        {
            yield return StartCoroutine(Pattern_Totem());
        }
    }
    //보스패턴 1 : 생존장판을 찾아서 들어가야 전체 즉사기를 피할 수 있음
    private IEnumerator Pattern_ZoneMixture()
    {
        Debug.Log("기믹 발동: 진짜 안전지대를 찾아라ㅋ");
        spawnedZones.Clear();
        
        int safeIndex = Random.Range(0, totalZones);
        for (int i = 0; i < totalZones; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnZoneDistance;
            int id = (i == safeIndex) ? safetyZoneID : damageZoneID;

            GameObject zone = PoolManager.instance.PoolDic[PoolType.BossPattern].Pop(id, spawnPos, Quaternion.identity);
            if (zone.TryGetComponent<BossPattern>(out BossPattern bossPattern))
            {
                spawnedZones.Add(bossPattern);
            }

        }
        float curTimer = alertTime;

        while (curTimer > 0)
        {
            curTimer -= Time.deltaTime;
            yield return null;
        }
        ExecuteUltimateAttack();

        foreach (var zone in spawnedZones)
        {
            if (zone == null) continue;
            PoolManager.instance.PoolDic[PoolType.BossPattern].ReturnPool(zone);
        }
        
        yield return new WaitForSeconds(1f);
    }

    private void ExecuteUltimateAttack()
    {

        if (GameManager.instance.Player.IsInvincible == false)
        {
            GameManager.instance.Player.Hit(100);
        }
        else
        {
            Debug.Log("플레이어가 안전지대에서 생존했습니다.");
        }
    }
    //보스패턴 2 : 토템을 부숴야 패턴이 끝날때 받는 체력 %데미지를 줄일 수 있음
    //총 1~4개 소환 남아있는 토템 당 전체체력 25%의 데미지
    
    private IEnumerator Pattern_Totem()
    {
        Debug.Log("토템을 부숴라");
        totemSpawnList.Clear();
        int randomCount = Random.Range(1, 5);
        for (int i = 0; i < randomCount; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnTotemDistance;
            
            GameObject go = PoolManager.instance.PoolDic[PoolType.BossPattern].Pop(totemID, spawnPos, Quaternion.identity);
            if(go.TryGetComponent<Totem>(out Totem totem))
            {
                totem.Init(OnTotemDie);
                totemSpawnList.Add(totem);
            }
        }
        aliveTotemCount = randomCount;
        float curTimer = alertTime_perTotems * randomCount;
        while (curTimer > 0 && aliveTotemCount >0)
        {
            curTimer -= Time.deltaTime;
            yield return null;
        }
        ExcuteTotemAttack(aliveTotemCount);

        foreach (var totem in totemSpawnList)
        {
            if (totem != null)
            {
                PoolManager.instance.PoolDic[PoolType.BossPattern].ReturnPool(totem);
            }
        }

        yield return new WaitForSeconds(1f);
    }
    private void OnTotemDie()
    {
        aliveTotemCount--;
    }
    private void ExcuteTotemAttack(int aliveTotemCount)
    {
        Debug.Log($"남아있는 토템의 개수 : {aliveTotemCount}");
        GameManager.instance.Player.Hit(25 * aliveTotemCount);
    }
}
