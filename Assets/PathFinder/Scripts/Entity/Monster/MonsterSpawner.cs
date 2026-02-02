using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster ID")]
    [SerializeField]
    private int monsterID;
    [Header("SpawnPoint")]
    [SerializeField]
    private GameObject spawnPoint;
    private Vector2 spawnVec;
    [Header("Visible Size")]
    [SerializeField]
    private float visibleSizeMultiply;

    private bool isSpawn;
    private bool isOnCamera;
    private GameObject spawnObj;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        spawnVec = spawnPoint.transform.position;
        transform.localScale = transform.localScale * visibleSizeMultiply;
    }
    private void OnBecameVisible()
    {
        isOnCamera = true;
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        if (!isSpawn)
        {
            Spawn();
        }

    }
    private void OnBecameInvisible()
    {
        isOnCamera = false;
        if (isSpawn && returnCoroutine == null)
        {
            returnCoroutine = StartCoroutine(ReturnPoolCo());
        }
    }
    public void Spawn()
    {
        isSpawn = true;
        spawnObj = PoolManager.instance.PoolDic[PoolType.Monster].Pop(monsterID, spawnVec, Quaternion.identity);
    }
    public IEnumerator ReturnPoolCo()
    {
        yield return new WaitForSeconds(3f);

        // 대기 시간 이후에도 여전히 카메라 밖에 있다면 반납
        if (!isOnCamera && isSpawn)
        {
            if(spawnObj.TryGetComponent<IPoolable>(out IPoolable go))
            {
                PoolManager.instance.PoolDic[PoolType.Monster].ReturnPool(go);
                spawnObj = null;
                isSpawn = false;
            }
        }

        returnCoroutine = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnVec, 0.5f);
    }
}
