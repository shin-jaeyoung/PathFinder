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
    private float[] checkAngles = { 15f,  30f, 45f , 60f,75f };
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

        //장애물 회피중?
        if (isAvoidanceMode)
        {
            RaycastHit2D targetCheck = Physics2D.Raycast(currentPos, targetDir, rayDistance , obstacleMask);
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
            foreach (float angle in checkAngles)
            {
                // avoidanceSide 방향을 먼저 검사 (-1이면 오른쪽, 1이면 왼쪽)
                float selectAngle = angle * avoidanceSide;
                Vector2 rotatedDir = Quaternion.Euler(0, 0, selectAngle) * targetDir;

                if (Physics2D.Raycast(currentPos, rotatedDir, rayDistance , obstacleMask).collider == null)
                {
                    SetAvoidance(rotatedDir);
                    return fixedAvoidDir;
                }

                selectAngle = angle * -avoidanceSide;
                rotatedDir = Quaternion.Euler(0, 0, selectAngle) * targetDir;

                if (Physics2D.Raycast(currentPos, rotatedDir, rayDistance , obstacleMask).collider == null)
                {
                    // 반대 방향이 비었다면 우선순위 방향을 교체
                    avoidanceSide *= -1f;
                    SetAvoidance(rotatedDir);
                    return fixedAvoidDir;
                }
            }
        }
        return targetDir;
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

        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
