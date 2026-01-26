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

    [Header("Move Range")]
    [SerializeField]
    private float moveRange;

    private Vector2 originVec;


    [Header("Mask")]
    [SerializeField]
    private LayerMask detectMask;
    [SerializeField]
    private LayerMask targetMask;

    [Header("Target")]
    [SerializeField]
    private Transform target;

    //property

    public Vector2 OriginVec => originVec;
    public Transform Target => target;
    public bool IsDetect => isDetect;
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
        return Vector2.Distance(transform.position, target.position) <= attackRange;
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
