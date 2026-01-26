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
    private float[] checkAngles = { 20f,  40f,  60f, 80f };
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
        Collider2D hit =  Physics2D.OverlapCircle(transform.position, detectRange, targetMask);
        if (hit == null)
        {
            isDetect = false;
            target = null;
            return;
        }
        //벽뒤는 감지 못하게
        Vector2 dir = (hit.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(hit.transform.position, transform.position);
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, dir, distance, detectMask);

        if(raycastHit.collider != null && ((1 << raycastHit.collider.gameObject.layer) & targetMask) != 0)
        {
            isDetect = true;
            target = raycastHit.transform;
        }
        else
        {
            isDetect = false;
            target = null;
        }

    }

    public IEnumerator DectCo()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while(true)
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
        if(target == null)
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

        // 1. 이미 회피 중이라면?
        if (isAvoidanceMode)
        {
            // 원래 가려던 방향(타겟 방향)에 장애물이 사라졌는지 체크
            RaycastHit2D targetCheck = Physics2D.Raycast(currentPos, targetDir, rayDistance + 0.5f, obstacleMask);

            if (targetCheck.collider == null)
            {
                // 이제 앞이 비었으니 회피 모드 종료!
                isAvoidanceMode = false;
                return targetDir;
            }

            // 아직 장애물이 있다면, 이전에 정해둔 회피 방향으로 계속 전진
            return fixedAvoidDir;
        }

        // 2. 회피 중이 아닐 때 장애물을 만난다면?
        RaycastHit2D hit = Physics2D.Raycast(currentPos, targetDir, rayDistance, obstacleMask);
        if (hit.collider != null)
        {
            // 좌우 탐색해서 한 방향을 '고정 회피 방향'으로 설정
            foreach (float angle in checkAngles)
            {
                Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * targetDir;
                RaycastHit2D checkHit = Physics2D.Raycast(currentPos, rotatedDir, rayDistance + 0.3f, obstacleMask);

                if (checkHit.collider == null)
                {
                    isAvoidanceMode = true;
                    fixedAvoidDir = rotatedDir; // 방향 고정!
                    return fixedAvoidDir;
                }
            }
        }

        // 3. 아무 일 없으면 그냥 타겟 방향으로
        return targetDir;
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

        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
