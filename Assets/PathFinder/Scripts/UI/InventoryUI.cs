using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField]
    private List<Image> equipments;

    [Header("Inventory")]
    [SerializeField]
    private GameObject content;

    [Header("Gold")]
    [SerializeField]
    private TextMeshProUGUI goldvalue;

    [Header("Explain")]
    [SerializeField]
    private GameObject explainPanel;
    [SerializeField]
    private TextMeshProUGUI explain;


    private Player player;
    private List<Image> inven;

    public void Init()
    {
        if (GameManager.instance.Player != null)
        {
            player = GameManager.instance.Player;
            inven = new List<Image>(player.Inventory.Capacity);
            for(int i = 0; i < player.Inventory.Capacity; i++)
            {
                Image icon = content.transform.GetChild(i).GetChild(0)?.GetComponent<Image>();
                inven.Add(icon);
            }
            player.Inventory.OnInventoryChaneged += UpdateInven;
            player.Inventory.OnEquipmentChanged += UpdateEquipment;
            player.Inventory.OnGoldChanged += UpdateGold;
            UpdateInven();
            UpdateEquipment();
            UpdateGold();
        }
    }
    private void Start()
    {
        Init();
    }
    private void OnEnable()
    {
        if (player == null) return;
        UpdateInven();
        UpdateEquipment();
        UpdateGold();
    }
    private void OnDestroy()
    {
        player.Inventory.OnInventoryChaneged -= UpdateInven;
        player.Inventory.OnEquipmentChanged -= UpdateEquipment;
        player.Inventory.OnGoldChanged -= UpdateGold;
    }
 
    public void UpdateInven()
    {
        for(int i = 0;i < player.Inventory.Capacity; i++)
        {
            if(player.Inventory.Inventory[i].IsEmpty() || player.Inventory.Inventory[i].item.Data.Sprite ==null)
            {
                inven[i].sprite = null;
                inven[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                inven[i].sprite = player.Inventory.Inventory[i].item.Data.Sprite;
                inven[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
    public void UpdateEquipment()
    {
        for(int i = 0;i < player.Inventory.Equipments.Count;i++)
        {
            if (player.Inventory.Equipments[i].IsEmpty() || player.Inventory.Equipments[i].item.Data.Sprite == null)
            {
                equipments[i].sprite = null;
                equipments[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                equipments[i].sprite = player.Inventory.Equipments[i].item.Data.Sprite;
                equipments[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void UpdateGold()
    {
        goldvalue.text = player.Inventory.Gold.ToString();
    }


    //만들고보니 이거 slot이 써야될것같은데 일단 보류
    public void UpdateExplain(InventorySlot slot)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(slot.item.Data.Name)
            .AppendLine()
            .AppendLine()
            .Append(slot.item.Data.Description);
        explain.text = sb.ToString();
    }
}
