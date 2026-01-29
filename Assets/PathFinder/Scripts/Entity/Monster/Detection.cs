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
    [SerializeField] private float moveRange;

    private Vector2 originVec;

    [Header("RayDistance")]
    [SerializeField] private float rayDistance;
    private float[] checkAngles = { 15f, 25f, 35f, 45f, 55f, 60f, 65f ,70f };
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

    [Header("Target & Visibility")]
    [SerializeField] 
    private Transform target;
    private Vector2 lastKnownPos;
    private bool isTargetVisible;

    [Header("Tracking Settings")]
    [SerializeField] 
    private float viewLostThreshold; // 수색 지속 시간
    private Coroutine trackWatchCoroutine;
    private bool isSearching = false;

    // Properties
    public Vector2 OriginVec => originVec;
    public Transform Target => target;
    public bool IsDetect => isDetect;
    public bool CanAttack => canAttack;
    public LayerMask ObstacleMask => obstacleMask;
    public Vector2 LastKnownPos => lastKnownPos;
    public bool IsTargetVisible => isTargetVisible;
    public bool IsSearching => isSearching;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        originVec = transform.position;
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

        Vector2 dir = (hit.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(hit.transform.position, transform.position);
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, dir, distance, detectMask);

        if (raycastHit.collider != null && ((1 << raycastHit.collider.gameObject.layer) & targetMask) != 0)
        {
            isDetect = true;
            target = raycastHit.transform;
            lastKnownPos = target.position+ new Vector3(0,0.5f,0);
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

        // 추적 거리나 이동 범위를 벗어나면 리셋
        if (distToTarget > trackRange || distFromOrigin > moveRange)
        {
            TargetReset();
        }
    }

    public void TargetReset()
    {
        isDetect = false;
        target = null;
        isTargetVisible = false;
        isSearching = false;
        StopTrackingWatch();
    }

    public bool IsInAttackRange()
    {
        if (target == null || !canAttack) return false;
        // 공격은 타겟이 실제로 보일 때만 가능하게 설정
        if (!isTargetVisible) return false;

        StartCoroutine(AttackCooltimeCo());
        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }

    public IEnumerator AttackCooltimeCo()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooltime);
        canAttack = true;
    }

    // 매 프레임 타겟이 벽 뒤에 있는지 체크하고 마지막 위치 업데이트
    public void UpdateVisibility()
    {
        if (target == null)
        {
            isTargetVisible = false;
            return;
        }

        Vector2 dir = (target.position - (Vector3)transform.position).normalized;
        float dist = Vector2.Distance(transform.position, target.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleMask);

        if (hit.collider == null)
        {
            isTargetVisible = true;
            lastKnownPos = target.position; 
        }
        else
        {
            isTargetVisible = false;
        }
    }

    public Vector2 GetAdjustedDirection(Vector3 targetPos)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetDir = ((Vector2)targetPos - currentPos).normalized;

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
        if (isSearching) return;
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
        isSearching = false;
    }

    // 마지막 위치에 도착했을 때 실행할 수색 코루틴
    private IEnumerator TrackWatchCo()
    {
        isSearching = true;
        float timer = 0f;
        WaitForSeconds checkInterval = new WaitForSeconds(0.2f);

        while (timer < viewLostThreshold)
        {
            UpdateVisibility();

            // 수색 도중 타겟이 다시 보이면 즉시 중단
            if (isTargetVisible)
            {
                Debug.Log("수색 중 재발견!");
                isSearching = false;
                yield break;
            }

            timer += 0.2f;
            yield return checkInterval;
        }

        Debug.Log("수색 실패: 타겟을 놓쳤습니다.");
        TargetReset();
    }

    private void SetAvoidance(Vector2 dir)
    {
        isAvoidanceMode = true;
        fixedAvoidDir = dir;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, trackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(originVec, moveRange);

        if (target != null)
        {
            Gizmos.color = isTargetVisible ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, target.position);

            if (!isTargetVisible)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(lastKnownPos, Vector3.one * 0.5f);
            }
        }
    }
}