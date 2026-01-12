using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInventoryUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField]
    private Button activeButton;
    [SerializeField]
    private Button passiveButton;
    [Header("UI Panel")]
    [SerializeField]
    private GameObject activePanel;
    [SerializeField]
    private GameObject passivePanel;


    [Header("Equipments")]
    [SerializeField]
    private GameObject equipContent;

    [Header("Inventory")]
    [SerializeField]
    private GameObject activeContent;
    [SerializeField]
    private GameObject passiveContent;

    [Header("Explain")]
    [SerializeField]
    private GameObject explainPanel;
    [SerializeField]
    private TextMeshProUGUI explainName;
    [SerializeField]
    private TextMeshProUGUI explainDescription;

    private Player player;
    private List<SkillSlotUI> activeSkillInven;
    private List<SkillSlotUI> passiveSkillInven;

    private List<SkillSlotUI> equipSkill;
    public void Init()
    {
        if(GameManager.instance.Player!=null)
        {
            player = GameManager.instance.Player;
            activeSkillInven = new List<SkillSlotUI>(player.Skills.ActiveCapacity);
            passiveSkillInven = new List<SkillSlotUI>(player.Skills.PassiveCapacity);
            equipSkill = new List<SkillSlotUI> (player.Skills.EquipCapacity);

            for (int i = 0; i < activeContent.transform.childCount; i++)
            {
                SkillSlotUI slotUI = activeContent.transform.GetChild(i).GetComponent<SkillSlotUI>();
                if(slotUI != null)
                {
                    slotUI.SetIndex(i);
                    slotUI.GetSlotData();
                    activeSkillInven.Add(slotUI);
                }
            }
            for(int i = 0; i < passiveContent.transform.childCount; i++)
            {
                SkillSlotUI slotUI = passiveContent.transform.GetChild(i).GetComponent<SkillSlotUI>();
                if (slotUI != null)
                {
                    slotUI.SetIndex(i);
                    slotUI.GetSlotData();
                    passiveSkillInven.Add(slotUI);
                }
            }
            for (int i = 0; i< equipContent.transform.childCount; i++)
            {
                SkillSlotUI slotUI = equipContent.transform.GetChild(i).GetComponent<SkillSlotUI>();
                if (slotUI != null)
                {
                    slotUI.SetIndex(i);
                    slotUI.GetSlotData();
                    equipSkill.Add(slotUI);
                }
            }
            explainName.text = "";
            explainDescription.text = "IF click Skill Icon, you can see the skill's description";
            UpdateAll();
        }
    }
    private void Start()
    {
        Init();
        activeButton.onClick.RemoveAllListeners();
        passiveButton.onClick.RemoveAllListeners();
        activeButton.onClick.AddListener(() => SwichTab(true));
        passiveButton.onClick.AddListener(()=>SwichTab(false));
        
    }
    private void OnEnable()
    {
        if (player == null) return;
        UpdateAll();
    }
    private void OnDestroy()
    {
        activeButton.onClick.RemoveAllListeners();
        passiveButton.onClick.RemoveAllListeners();
    }
    public void SwichTab(bool isActive)
    {
        activePanel.SetActive(isActive);
        passivePanel.SetActive(!isActive);
    }
    
    public void UpdateAll()
    {
        foreach(var slot in equipSkill)
        {
            slot.UpdateUI();
        }
        foreach(var slot in activeSkillInven)
        {
            slot.UpdateUI();
        }
        foreach(var slot in passiveSkillInven)
        {
            slot.UpdateUI();
        }
    }
    public void UpdateExplain(SkillSlot activeSlot)
    {
        explainName.text = activeSlot.skill.Data.SkillName.ToString();
        explainDescription.text = activeSlot.skill.Data.Description.ToString();
    }
    public void UpdateExplain(PassiveSlot slot)
    {
        explainName.text = slot.passiveSkill.Data.Name.ToString();
        explainDescription.text = slot.passiveSkill.Data.Description.ToString();
    }
}
