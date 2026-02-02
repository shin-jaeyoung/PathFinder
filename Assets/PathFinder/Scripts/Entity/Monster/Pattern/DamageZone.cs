using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : BossPattern
{
    public float coolTime;
    [Header("Reduce Hp")]
    public int percent;

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
}
