using UnityEngine;

public class DamageTextUI : MonoBehaviour
{
    [SerializeField] private int damageTextPoolID;
    private Camera mainCam;

    private void Awake() => mainCam = Camera.main;

    private void OnEnable() => GlobalEvents.OnDamage += SpawnDamageText;
    private void OnDisable() => GlobalEvents.OnDamage -= SpawnDamageText;

    private void SpawnDamageText(string damage, Vector2 worldPos)
    {
        GameObject go = PoolManager.instance.PoolDic[PoolType.DamageText].Pop(damageTextPoolID, Vector3.zero, Quaternion.identity);

        go.transform.SetParent(transform, false);

        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
        go.transform.position = screenPos;

        if (go.TryGetComponent(out DamageText textScript))
        {
            textScript.Init(damage);
        }
    }
}