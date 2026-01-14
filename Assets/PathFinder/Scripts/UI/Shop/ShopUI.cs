using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject prefab;
    public Transform content;
    //내부용
    public List<ShopSlotUI> shopSlots;
    public ShopSO shopSO;

    private void OnEnable()
    {
        if (ShopManager.instance == null || ShopManager.instance.CurShopNpc == null) return;
        
        if(ShopManager.instance.CurShopNpc !=null)
        {
            shopSO = ShopManager.instance.CurShopNpc.shopSO;
        }
        shopSlots = new List<ShopSlotUI>(shopSO.items.Count);
        for(int i = 0; i < shopSO.items.Count; i++)
        {
            GameObject go = Instantiate(prefab, content);
            ShopSlotUI newShopSlot = go.GetComponent<ShopSlotUI>();
            newShopSlot.item = shopSO.items[i];
            shopSlots.Add(newShopSlot);
        }
        RefreshUI();
    }
    
    private void OnDisable()
    {
        for(int i = 0; i< content.transform.childCount;i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        shopSlots.Clear();
        shopSO = null;
    }

    
    public void RefreshUI()
    {
        for(int i = 0; i < shopSlots.Count; i++)
        {
            shopSlots[i].RefreshUI();
        }
    }
}
