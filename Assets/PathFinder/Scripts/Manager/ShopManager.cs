using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    private Npc curNpc;
    public ShopNpc curShopNpc;

    public Item selectedItem;
    public int count;

    private Player player;

    //property

    public Npc CurNpc => curNpc;
    public ShopNpc CurShopNpc => curShopNpc;

    private void Awake()
    {
        if(instance == null)
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
        if(GameManager.instance.Player !=null)
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
    public void Sell(int index, int count = 1)
    {
        int gold = selectedItem.Data.Price;
        player.Inventory.AddGold(gold * count);
        player.Inventory.Inventory[index].Clear();
        selectedItem = null;
    }
    public void Buy(int count = 1)
    {
        int gold = selectedItem.Data.Price;
        if(player.Inventory.ReduceGold(gold * count))
        {
            player.Inventory.AddItem(selectedItem);
        }
        selectedItem = null;
    }
}
