using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyZone : MonoBehaviour,IPoolable
{
    [Header("Pooldata")]
    [SerializeField]
    private int id;
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetID()
    {
        return id;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            player.IsInvincible = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            player.IsInvincible = false;
        }
    }
}
