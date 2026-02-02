using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationText : MonoBehaviour, IPoolable
{
    [SerializeField] 
    private TextMeshProUGUI textMesh;
    [SerializeField] 
    private int id;

    private Coroutine returnRoutine;
    private bool isReturned = false; // 중복 반환 방지 플래그

    public int GetID() => id;
    public GameObject GetGameObject() => gameObject;

    public void SetText(string message, float duration)
    {
        textMesh.text = message;
        textMesh.alpha = 1f; 
        isReturned = false;

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
        }
        returnRoutine = StartCoroutine(ReturnToPoolAfterDelay(duration));
    }
    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DoReturn();
    }

    private void OnDisable()
    {
        // HUD가 꺼지는 등 외부 요인으로 비활성화될 때 즉시 반환
        DoReturn();

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
    }

    private void DoReturn()
    {
        if (isReturned) return; 

        isReturned = true;

        PoolManager.instance.PoolDic[PoolType.UI].ReturnPool(this);
    }
}