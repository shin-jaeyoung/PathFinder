using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopNpc : SpecialNpc, ISpecialInteractable
{
    public ShopSO shopSO;
    public bool isShopOpen = false;

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
        UIManager.Instance.Showonly(UIType.Shop);
        isShopOpen = true;
    }
    public void CloseShop()
    {
        UIManager.Instance.HideUI(UIType.Shop);
        isShopOpen = false;
        isInteractFinish = true;
    }
    public void Init()
    {
        UIManager.Instance.Showonly(UIType.HUD);
        isShopOpen = false;
        isInteractFinish = false;
    }
    public void NoticeData(ShopSO shopSO)
    {

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
