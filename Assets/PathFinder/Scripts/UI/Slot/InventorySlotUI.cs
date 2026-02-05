using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : SlotUI ,IPointerEnterHandler,IPointerExitHandler
{
    [Header("Inventory Specific")]
    [SerializeField] 
    private TextMeshProUGUI countText;

    private int slotIndex;
    private InventoryUI ui;


    public int SlotIndex => slotIndex;
    public TextMeshProUGUI CountText => countText;

    public void SetIndex(int index)
    {
        slotIndex = index;
    }


    public InventorySlot GetSlotData()
    {
        return player.Inventory.Inventory[slotIndex];
    }

    private void Start()
    {
        ui = GetComponentInParent<InventoryUI>();
        player.Inventory.OnInventoryChaneged += UpdateUI;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        InventorySlot data = GetSlotData();

        if (data.IsEmpty() || data.item.Data.Sprite == null)
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            group.alpha = 0;
        }
        else
        {
            icon.sprite = data.item.Data.Sprite;
            icon.color = Color.white;
            group.alpha = 1;
            if (countText != null)
            {
                if (data.count > 1)
                {
                    countText.text = data.count.ToString();
                    countText.gameObject.SetActive(true);
                }
                else
                {
                    countText.gameObject.SetActive(false);
                }
            }
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI sourceSlotUI = eventData.pointerDrag?.GetComponent<InventorySlotUI>();

        if (sourceSlotUI != null && sourceSlotUI != this)
        {
            SwapItems(sourceSlotUI.SlotIndex, this.SlotIndex);
            iconRect.anchoredPosition = Vector2.zero;
            return;
        }

        EquipmentSlotUI equipSlotUI = eventData.pointerDrag?.GetComponent<EquipmentSlotUI>();
        if (equipSlotUI != null)
        {
            player.Inventory.RemoveEquipment(equipSlotUI.SlotIndex);
            iconRect.anchoredPosition = Vector2.zero;
        }
    }

    private void SwapItems(int startIndex, int targetIndex)
    {

        var inven = player.Inventory.Inventory;

        InventorySlot temp = inven[startIndex];

        Item tempItem = inven[startIndex].item;
        int tempCount = inven[startIndex].count;

        inven[startIndex].item = inven[targetIndex].item;
        inven[startIndex].count = inven[targetIndex].count;

        inven[targetIndex].item = tempItem;
        inven[targetIndex].count = tempCount;


        player.Inventory.OnInventoryChaneged?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;

        ui?.ExplainReomote(true);
        ui?.UpdateExplain(GetSlotData(), transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetSlotData().IsEmpty()) return;

        ui?.ExplainReomote(false);
    }
}