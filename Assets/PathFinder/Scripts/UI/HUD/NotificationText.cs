using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int id;

    private Coroutine returnRoutine;

    public int GetID() => id;
    public GameObject GetGameObject() => gameObject;

    public void SetText(string message, float duration)
    {
        textMesh.text = message;

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
        }
        returnRoutine = StartCoroutine(ReturnToPoolAfterDelay(duration));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        returnRoutine = null;
        if (gameObject.activeSelf)
        {
            PoolManager.instance.PoolDic[PoolType.UI].ReturnPool(this);
        }
    }

    private void OnDisable()
    {
        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
    }
}