using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossMonster : Monster
{

    [Header("Gimmick Settings")]
    [SerializeField]
    private float gimmickInterval;
    private bool isGimmickActive = false;
    [Header("Pattern_FindSafetyZone")]
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
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 6f;
            //풀링하게 되면 여기가 패턴 ID라던가 그럴듯
            GameObject prefab = (i == safeIndex) ? safetyZonePrefab : damageZonePrefab;
            GameObject zone = Instantiate(prefab, spawnPos, Quaternion.identity);
            spawnedZones.Add(zone);

        }
        float curTimer = alertTime;

        while (curTimer > 0)
        {
            curTimer -= Time.deltaTime;
            yield return null;
        }
        ExecuteUltimateAttack();

        foreach (var zone in spawnedZones) Destroy(zone);
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
    [Header("Pattern_Totem")]
    [SerializeField]
    private Totem totemPrefab;
    [SerializeField]
    private float spawnDistance;
    [SerializeField]
    private float alertTime_perTotems;
    private List<Totem> totemSpawnList = new List<Totem>();
    
    private IEnumerator Pattern_Totem()
    {
        Debug.Log("토템을 부숴라");
        totemSpawnList.Clear();
        int randomCount = Random.Range(1, 5);
        for (int i = 0; i < randomCount; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnDistance;
            //풀링으로 만들때 ID로 가져와볼까 패턴 ID?
            //풀링은 나중에 다만들고 나서 바꾸자
            Totem zone = Instantiate(totemPrefab, spawnPos, Quaternion.identity);
            totemSpawnList.Add(zone);
        }
        int aliveTotemCount = randomCount;
        float curTimer = alertTime_perTotems * randomCount;
        while (curTimer > 0 )
        {
            curTimer -= Time.deltaTime;
            int tmpNum = 0; 
            foreach( Totem totem in  totemSpawnList )
            {
                if(totem != null && !totem.isDead)
                {
                    tmpNum++;
                }
            }
            aliveTotemCount = tmpNum;
            if (aliveTotemCount <= 0) break;
            yield return null;
        }
        ExcuteTotemAttack(aliveTotemCount);

        foreach (var totem in totemSpawnList)
        {
            if (totem != null)
            {
                Destroy(totem.gameObject);
            }
        }

        yield return new WaitForSeconds(1f);
    }

    private void ExcuteTotemAttack(int aliveTotemCount)
    {
        Debug.Log($"남아있는 토템의 개수 : {aliveTotemCount}");
        GameManager.instance.Player.Hit(25 * aliveTotemCount);
    }
}
