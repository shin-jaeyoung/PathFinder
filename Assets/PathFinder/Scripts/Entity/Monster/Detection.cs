using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Detection : MonoBehaviour
{
    [Header("Perform Value")]
    [SerializeField]
    private float detectRange;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float trackRange;
    private bool isDetect;

    [Header("Attack Cooltime")]
    [SerializeField]
    private float attackCooltime;
    private bool canAttack = true;

    [Header("Move Range")]
    [SerializeField]
    private float moveRange;

    private Vector2 originVec;
    [Header("RayDistance")]
    [SerializeField]
    private float rayDistance;
    private float[] checkAngles = { 15f ,25f,35f ,45f,55f ,60f,65f,75f,80f };
    private float avoidanceSide = 1f;
    private bool isAvoidanceMode = false;
    private Vector2 fixedAvoidDir;

    [Header("Mask")]
    [SerializeField]
    private LayerMask detectMask;
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;

    [Header("Target")]
    [SerializeField]
    private Transform target;

    [Header("Tracking Settings")]
    [SerializeField] private float viewLostThreshold;
    private Coroutine trackWatchCoroutine;

    //property

    public Vector2 OriginVec => originVec;
    public Transform Target => target;
    public bool IsDetect => isDetect;
    public bool CanAttack => canAttack;
    public LayerMask ObstacleMask => obstacleMask;
    private void Awake()
    {
        originVec = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine(DectCo());
    }


    public void Detect()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, targetMask);
        if (hit == null)
        {
            TargetReset();
            return;
        }
        //벽뒤는 감지 못하게
        Vector2 dir = (hit.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(hit.transform.position, transform.position);
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, dir, distance, detectMask);

        if (raycastHit.collider != null && ((1 << raycastHit.collider.gameObject.layer) & targetMask) != 0)
        {
            isDetect = true;
            target = raycastHit.transform;
        }
        else
        {
            TargetReset();
        }

    }

    public IEnumerator DectCo()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            if (!isDetect)
            {
                Detect();
            }
            else
            {
                CheckTarget();
            }
            yield return wait;
        }
    }

    public void CheckTarget()
    {
        if (target == null)
        {
            TargetReset();
            return;
        }
        float distToTarget = Vector2.Distance(transform.position, target.position);
        float distFromOrigin = Vector2.Distance(originVec, transform.position);
        if (distToTarget > trackRange || distFromOrigin > moveRange)
        {
            TargetReset();
        }

    }

    public void TargetReset()
    {
        isDetect = false;
        target = null;
    }

    public bool IsInAttackRange()
    {
        if (target == null) return false;
        if (canAttack == false) return false;
        StartCoroutine(AttackCooltimeCo());

        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }
    public IEnumerator AttackCooltimeCo()
    {
        WaitForSeconds cooltime = new WaitForSeconds(attackCooltime);
        canAttack = false;
        yield return cooltime;
        canAttack = true;
    }


    public Vector2 GetAdjustedDirection(Vector3 targetPos)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetDir = ((Vector2)targetPos - currentPos).normalized;

        //장애물 회피중?
        if (isAvoidanceMode)
        {
            RaycastHit2D targetCheck = Physics2D.Raycast(currentPos, targetDir, rayDistance, obstacleMask);
            if (targetCheck.collider == null)
            {
                isAvoidanceMode = false;
                return targetDir;
            }
            return fixedAvoidDir;
        }
        RaycastHit2D hit = Physics2D.Raycast(currentPos, targetDir, rayDistance, obstacleMask);
        if (hit.collider != null)
        {
            float longestDistance = -1f;
            Vector2 bestDir = targetDir;

            foreach (float angle in checkAngles)
            {
                float[] sides = { angle, -angle };
                foreach (float s in sides)
                {
                    Vector2 scanDir = Quaternion.Euler(0, 0, s) * targetDir;
                    RaycastHit2D scanHit = Physics2D.Raycast(currentPos, scanDir, 10f, obstacleMask);

                    float currentDist;
                    if (scanHit.collider == null)
                    {
                        SetAvoidance(scanDir);
                        return scanDir;
                    }
                    else
                    {
                        currentDist = Vector2.Distance(currentPos, scanHit.point);
                    }

                    // 현재까지 본 방향 중 가장 멀리 뚫린 방향 업데이트
                    if (currentDist > longestDistance)
                    {
                        longestDistance = currentDist;
                        bestDir = scanDir;
                        avoidanceSide = (s > 0) ? 1f : -1f;
                    }
                }
            }
            SetAvoidance(bestDir);
            return bestDir;
        }
        return targetDir;
    }

    public void StartTrackingWatch()
    {
        StopTrackingWatch(); 
        trackWatchCoroutine = StartCoroutine(TrackWatchCo());
    }
    public void StopTrackingWatch()
    {
        if (trackWatchCoroutine != null)
        {
            StopCoroutine(trackWatchCoroutine);
            trackWatchCoroutine = null;
        }
    }

    //추적중 레이로 벽뒤로 숨었는지 체크
    //일정시간동안 플레이어 감지 실패시 복귀
    private IEnumerator TrackWatchCo()
    {
        float hiddenTimer = 0f;

        WaitForSeconds checkInterval = new WaitForSeconds(0.5f);

        while (isDetect && target != null)
        {
            if (IsTargetHiddenByWall())
            {
                hiddenTimer += 0.5f; 
                Debug.Log($"플레이어 은폐 중... ({hiddenTimer}s)");
                
                if (hiddenTimer >= viewLostThreshold)
                {
                    Debug.Log("놓쳤음 복귀 ㄱ.");
                    TargetReset();
                    yield break; // 코루틴 종료
                }
            }
            else
            {
                hiddenTimer = 0f; // 다시 보이면 타이머 초기화
            }

            yield return checkInterval;
        }
    }

    private bool IsTargetHiddenByWall()
    {
        if (target == null) return true;

        Vector2 dir = (target.position - transform.position).normalized;
        float dist = Vector2.Distance(transform.position, target.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleMask);

        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void SetAvoidance(Vector2 dir)
    {
        isAvoidanceMode = true;
        fixedAvoidDir = dir;
    }
    private void OnDrawGizmosSelected()
    {
        // 탐지 범위 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //추적 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, trackRange);

        //이동가능 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(originVec, moveRange);

        //타겟에 쏘는 레이
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
