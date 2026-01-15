using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    private Npc curNpc;
    public ShopNpc curShopNpc;
    public bool isSell;
    public Item selectedItem;
    public int playerinvenIndex;
    public int count;

    private Player player;

    //property

    public Npc CurNpc => curNpc;
    public ShopNpc CurShopNpc => curShopNpc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (GameManager.instance.Player != null)
        {
            player = GameManager.instance.Player;
        }
    }
    public void SetNpc(Npc npc, ShopNpc shopNpc)
    {
        curNpc = npc;
        curShopNpc = shopNpc;
    }
    public void ClearNpc()
    {
        curNpc = null;
        curShopNpc = null;
    }
    public void ClearItemInfo()
    {
        selectedItem = null;
        playerinvenIndex = -1;
        count = 1;
    }
    public void Sell()
    {
        int gold = selectedItem.Data.Price;
        if(player.Inventory.RemoveItem(player.Inventory.Inventory[playerinvenIndex], count))
        {
            player.Inventory.AddGold(gold * count);
            Debug.Log("판매성공");
        }

        ClearItemInfo();
    }
    public void Buy()
    {
        int gold = selectedItem.Data.Price;
        Debug.Log($"구매가격 : {gold}");
        if (player.Inventory.ReduceGold(gold * count))
        {
            player.Inventory.AddItem(selectedItem);
            Debug.Log("구매성공");
        }
        ClearItemInfo();
    }
}
