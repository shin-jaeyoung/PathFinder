using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour,IPoolable
{
    public float coolTime;
    [Header("Reduce Hp")]
    public int percent;
    [Header("Pooldata")]
    [SerializeField]
    private int id;
    private Coroutine damageRoutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            damageRoutine = StartCoroutine(DamageCooltimeCo(player));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if( damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }

    private void OnDisable()
    {
        if (damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }
    private IEnumerator DamageCooltimeCo(Player player)
    {
        while(true)
        {
            player.Hit(percent);
            yield return new WaitForSeconds(coolTime);
        }

    }

    public int GetID()
    {
        return id;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
