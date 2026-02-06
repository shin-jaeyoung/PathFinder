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
    private SceneType spawnerScene;
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
        if(GameManager.instance.CurScene != spawnerScene)
        {
            ReturnPool();
        }
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        if (!isSpawn && GameManager.instance.CurScene == spawnerScene)
        {
            Spawn();
        }

    }
    private void OnBecameInvisible()
    {
        isOnCamera = false;
        if (isActiveAndEnabled&&isSpawn && returnCoroutine == null)
        {
            returnCoroutine = StartCoroutine(ReturnPoolCo());
        }
    }
    public void Spawn()
    {
        isSpawn = true;
        spawnObj = PoolManager.instance.PoolDic[PoolType.Monster].Pop(monsterID, spawnVec, Quaternion.identity);
        spawnObj.transform.SetParent(transform, true);
    }
    public IEnumerator ReturnPoolCo()
    {
        yield return new WaitForSeconds(3f);

        // 대기 시간 이후에도 여전히 카메라 밖에 있다면 반납
        ReturnPool();

    }
    private void ReturnPool()
    {
        if (!isOnCamera && isSpawn)
        {
            if (spawnObj.TryGetComponent<IPoolable>(out IPoolable go))
            {
                PoolManager.instance.PoolDic[PoolType.Monster].ReturnPool(go);
                spawnObj.transform.SetParent(PoolManager.instance.PoolParentDic[PoolType.Monster].transform, true);
                isSpawn = false;
            }
        }
        returnCoroutine = null;
    }
    private void OnDestroy()
    {
        if(isSpawn)
        {
            ReturnPool();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnVec, 0.5f);
    }
}
