using UnityEngine;
using TMPro;
using System.Collections;

public class DamageText : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int poolID;
    private const float duration = 1f;

    public int GetID() => poolID;
    public GameObject GetGameObject() => gameObject;

    public void Init(string damage)
    {
        textMesh.text = damage;
        textMesh.alpha = 1f;
        transform.localScale = Vector3.one;

        StopAllCoroutines();
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        //데미지 텍스트가 조금 위에 있어야할듯?
        Vector3 targetPos = startPos + new Vector3(0, 100f, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            //점점 사라지는 효과
            if (t > 0.5f)
            {
                textMesh.alpha = Mathf.Lerp(1f, 0f, (t - 0.5f) * 2f);
            }
            //초반엔 좀 크게 만드는 효과 약간 액션성을 부여하는거?
            if (t < 0.2f)
            {
                transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, t / 0.2f);
            }

            yield return null;
        }

        PoolManager.instance.PoolDic[PoolType.UI].ReturnPool(this);
    }
}