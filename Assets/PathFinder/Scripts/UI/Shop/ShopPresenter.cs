using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShopPresenter : MonoBehaviour
{
    [SerializeField]
    private ShopUI shopUI;
    [SerializeField]
    private ShopInvenUI shopInvenUI;
    [SerializeField]
    private ShopPlayerGold goldUI;
    [SerializeField]
    private ShopExplainUI explainUI;
    [SerializeField]
    private ShopCheckUI checkUI;


    StringBuilder sb = new StringBuilder();

    private Player player;
    private IEnumerator WaitCO()
    {
        while(shopUI.shopSO ==null)
        {
            yield return null;
        }
        ShopUISet();
    }
    public void ShopUISet()
    {
        if (ShopManager.instance != null && ShopManager.instance.CurNpc != null)
        {
            for (int i = 0; i < shopUI.shopSlots.Count; i++)
            {
                int currentIndex = i;
                shopUI.shopSlots[i].OnPointerEntered += () =>
                {

                    if (shopUI.shopSlots[currentIndex].IsEmpty())
                    {
                        explainUI.gameObject.SetActive(false);
                        return;
                    }
                    sb.Clear();
                    sb.Append("Name : ").Append(shopUI.shopSlots[currentIndex].item.Data.Name)
                        .AppendLine().Append("Price : ").Append(shopUI.shopSlots[currentIndex].item.Data.Price)
                        .AppendLine().Append(shopUI.shopSlots[currentIndex].item.Data.Description);

                    explainUI.RefreshUI(sb.ToString());
                    explainUI.transform.position = shopUI.shopSlots[currentIndex].transform.position;
                    explainUI.gameObject.SetActive(true);
                };
                shopUI.shopSlots[i].OnPointerExitted += () =>
                {
                    explainUI.gameObject.SetActive(false);
                };
                shopUI.shopSlots[i].OnPointerClicked += () =>
                {
                    if (!shopUI.shopSlots[currentIndex].IsEmpty())
                    {
                        //TODO : Shopmanager의 커런트아이템(셀렉아이템)을 할당해주고
                        checkUI.gameObject.SetActive(true);
                    }
                };
            }

            shopUI.RefreshUI();
        }
    }
    private void OnEnable()
    {
        checkUI.gameObject.SetActive(false);
        explainUI.gameObject.SetActive(false);
        //ShopUI
        StartCoroutine(WaitCO());
    }
    private void Start()
    {
        player = GameManager.instance.Player;
        player.Inventory.OnInventoryChaneged += RefreshShopInvenUI;
        player.Inventory.OnGoldChanged += RefreshGold;
        
        
        //ShopInventory UI
        for (int i = 0; i<shopInvenUI.shopinvenSlots.Count; i++)
        {
            int currentIndex = i;
            
            shopInvenUI.shopinvenSlots[i].OnPointerEntered += () =>
            {

                if (player.Inventory.Inventory[currentIndex].IsEmpty())
                {
                    explainUI.gameObject.SetActive(false);
                    return;
                }
                sb.Clear();
                sb.Append("Name : ").Append(player.Inventory.Inventory[currentIndex].item.Data.Name)
                    .AppendLine().Append("Price : ").Append(player.Inventory.Inventory[currentIndex].item.Data.Price)
                    .AppendLine().Append(player.Inventory.Inventory[currentIndex].item.Data.Description);

                explainUI.RefreshUI(sb.ToString());
                explainUI.transform.position = shopInvenUI.shopinvenSlots[currentIndex].transform.position;
                explainUI.gameObject.SetActive(true);
            };
            shopInvenUI.shopinvenSlots[i].OnPointerExitted += () =>
            {
                explainUI.gameObject.SetActive(false);
            };
            shopInvenUI.shopinvenSlots[i].OnPointerClicked += () =>
            {
                if (!player.Inventory.Inventory[currentIndex].IsEmpty())
                {
                    //TODO : Shopmanager의 커런트아이템(셀렉아이템)을 할당해주고
                    checkUI.gameObject.SetActive(true);
                }
            };
        }

        RefreshShopInvenUI();
        RefreshGold();
    }
    
    private void OnDestroy()
    {
        player.Inventory.OnInventoryChaneged -= RefreshShopInvenUI;
        player.Inventory.OnGoldChanged -= RefreshGold;
        //람다식은 해제를 어떻게 하지?
    }
    public void RefreshShopInvenUI()
    {
        shopInvenUI.RefreshUI(player.Inventory.Inventory);
    }
    public void RefreshGold()
    {
        goldUI.RefreshUI(player.Inventory.Gold);
    }
}
