using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopNpc : SpecialNpc, ISpecialInteractable
{
    public Npc npc;
    public ShopSO shopSO;
    public bool isShopOpen = false;

    public void SetNpc(Npc npc)
    {
        this.npc = npc;
    }
    public override void SpecialInteract()
    {
        if (isShopOpen)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }
    public void OpenShop()
    {
        Debug.Log("상점오픈");
        //Shopmanager한테 본인 정보 넘겨줘야함
        ShopManager.instance.SetNpc(npc, this);

        UIManager.Instance.Showonly(UIType.Shop);
        isShopOpen = true;
    }
    public void CloseShop()
    {
        ShopManager.instance.ClearNpc();
        if(UIManager.Instance.CheckCurUIType(UIType.Shop))
        {
            UIManager.Instance.HideUI(UIType.Shop);
        }
        isShopOpen = false;
        isInteractFinish = true;
    }
    public void Init()
    {
        UIManager.Instance.ShowOnlyHUD();
        isShopOpen = false;
        isInteractFinish = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.GetComponent<Player>()!=null)
            {
                Init();
            }
        }
    }
}
