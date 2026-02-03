using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour, IPoolable
{
    [SerializeField] 
    private TextMeshProUGUI damageText;
    [SerializeField] 
    private int poolID;
    [SerializeField]
    float heightOffset;
    private const float duration = 1f;

    public int GetID()
    {
        return poolID;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Init(string damage, Transform target)
    {
        damageText.text = damage;
        damageText.alpha = 1f;
        transform.localScale = Vector3.one;

        StopAllCoroutines();
        StartCoroutine(FollowTargetRoutine(target));
    }

    private IEnumerator FollowTargetRoutine(Transform target)
    {
        float elapsed = 0f;
        Camera mainCam = Camera.main;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (target != null)
            {
                // 월드 좌표 + 상승 애니메이션 값을 스크린 좌표로 변환
                Vector3 worldPos = target.position + new Vector3(0, heightOffset + (t * 1.0f), 0);
                transform.position = mainCam.WorldToScreenPoint(worldPos);
            }

            if (t > 0.5f)
                damageText.alpha = Mathf.Lerp(1f, 0f, (t - 0.5f) * 2f);

            yield return null;
        }

        PoolManager.instance.PoolDic[PoolType.DamageText].ReturnPool(this);
    }
}