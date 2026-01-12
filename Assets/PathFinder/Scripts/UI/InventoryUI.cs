using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InventoryUI : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField]
    private List<EquipmentSlotUI> equipmentSlots;

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
    private List<InventorySlotUI> invenSlotUIs = new List<InventorySlotUI>();

    public void Init()
    {
        if (GameManager.instance.Player != null)
        {
            player = GameManager.instance.Player;

            invenSlotUIs.Clear();
            for (int i = 0; i < content.transform.childCount; i++)
            {
                InventorySlotUI slotUI = content.transform.GetChild(i).GetComponent<InventorySlotUI>();
                if (slotUI != null)
                {
                    slotUI.SetIndex(i);
                    InventorySlot data = slotUI.GetSlotData();
                    if (data.count > 1)
                    {
                        slotUI.CountText.text = data.count.ToString();
                        slotUI.CountText.gameObject.SetActive(true);
                    }
                    else
                    {
                        slotUI.CountText.gameObject.SetActive(false);
                    }
                    invenSlotUIs.Add(slotUI);
                }
            }

            
            player.Inventory.OnGoldChanged += UpdateGold;

            RefreshAll();
        }
    }
    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        if (player == null) return;
        RefreshAll();
    }

    private void OnDestroy()
    {
        if (player == null) return;
        player.Inventory.OnGoldChanged -= UpdateGold;
    }

    public void RefreshAll()
    {
        // 인벤토리 슬롯들 갱신
        foreach (var slot in invenSlotUIs)
        {
            slot.UpdateUI();
        }

        // 장비 슬롯들 갱신
        foreach (var equipSlot in equipmentSlots)
        {
            equipSlot.UpdateUI();
        }

        UpdateGold();
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
    public void UpdateExplain(EquipmentsSlot slot)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(slot.item.Data.Name)
            .AppendLine()
            .AppendLine()
            .Append(slot.item.Data.Description);
        explain.text = sb.ToString();
    }
    public void ExplainReomote(bool onoff)
    {
        explainPanel.SetActive(onoff);
    }
}
