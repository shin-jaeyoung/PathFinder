using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer worldSprite;
    [SerializeField]
    private Item item;
    private void Awake()
    {
        worldSprite = GetComponent<SpriteRenderer>();
    }
    public void Init(Item item)
    {
        this.item = item;
        worldSprite.sprite = item.Data.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            if (player.Inventory.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
