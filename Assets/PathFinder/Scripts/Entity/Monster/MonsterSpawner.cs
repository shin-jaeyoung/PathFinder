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

    [Header("Visible Size")]
    [SerializeField]
    private float visibleSizeMultiply;
    [SerializeField]
    private bool isSpawn;
    [SerializeField]
    private bool isOnCamera;
    [SerializeField]
    private GameObject spawnObj;
    private Coroutine returnCoroutine;

    private void Awake()
    {
        transform.localScale = transform.localScale * visibleSizeMultiply;
    }
    private void OnBecameVisible()
    {
        isOnCamera = true;
        if(GameManager.instance.CurScene != spawnerScene)
        {
            Debug.Log("현재씬과 스포너의 스폰씬이 다릅니다");
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
        spawnObj = PoolManager.instance.PoolDic[PoolType.Monster].Pop(monsterID, transform.position, Quaternion.identity);
        spawnObj.transform.SetParent(transform, true);
    }
    public IEnumerator ReturnPoolCo()
    {
        yield return new WaitForSeconds(3f);

        // 대기 시간 이후에도 여전히 카메라 밖에 있다면 리턴풀
        if (!isOnCamera)
        {
            if (spawnObj != null && spawnObj.activeSelf)
            {
                Debug.Log($"{gameObject.name}가 몬스터를 돌려보냅니다");
                ReturnPool();
            }
        }

        returnCoroutine = null;

    }
    private void ReturnPool()
    {
        if (spawnObj != null && isSpawn)
        {
            if (spawnObj.TryGetComponent<IPoolable>(out IPoolable go))
            {
                isSpawn = false;
                PoolManager.instance.PoolDic[PoolType.Monster].ReturnPool(go);
                
                spawnObj = null;
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
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
