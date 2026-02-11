using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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


    private StringBuilder sb = new StringBuilder();
    private Player player;
    private List<SkillSlotUI> activeSkillInven;
    private List<PassiveSkillSlotUI> passiveSkillInven;

    private List<SkillSlotUI> equipSkill;
    public void Init()
    {
        if(GameManager.instance.Player!=null)
        {
            player = GameManager.instance.Player;
            activeSkillInven = new List<SkillSlotUI>(player.Skills.ActiveCapacity);
            passiveSkillInven = new List<PassiveSkillSlotUI>(player.Skills.PassiveCapacity);
            equipSkill = new List<SkillSlotUI> (player.Skills.EquipCapacity);

            for (int i = 0; i < activeContent.transform.childCount; i++)
            {
                SkillSlotUI slotUI = activeContent.transform.GetChild(i).GetComponent<SkillSlotUI>();
                if(slotUI != null)
                {
                    slotUI.SetIndex(i);
                    
                    activeSkillInven.Add(slotUI);
                }
            }
            for(int i = 0; i < passiveContent.transform.childCount; i++)
            {
                PassiveSkillSlotUI slotUI = passiveContent.transform.GetChild(i).GetComponent<PassiveSkillSlotUI>();
                if (slotUI != null)
                {
                    slotUI.SetIndex(i);
                    
                    passiveSkillInven.Add(slotUI);
                }
            }
            for (int i = 0; i< equipContent.transform.childCount; i++)
            {
                SkillSlotUI slotUI = equipContent.transform.GetChild(i).GetComponent<SkillSlotUI>();
                if (slotUI != null)
                {
                    slotUI.SetIndex(i);
                    
                    equipSkill.Add(slotUI);
                }
            }
            explainName.text = "";
            explainDescription.text = "스킬아이콘을 누르면 설명이 나와요";
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
        explainName.text = "";
        explainDescription.text = "";
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
        explainName.text = activeSlot.skill.Data.SkillName;
        sb.Clear();
        sb.Append("스킬 데미지 계수 : ").Append(activeSlot.skill.Data.DamageMultiplier * 100).Append("%").AppendLine()
            .Append("쿨타임 : ").Append(activeSlot.skill.Data.Cooltime).Append("초").AppendLine()
            .Append(activeSlot.skill.Data.Description);


        explainDescription.text = sb.ToString();
    }
    public void UpdateExplain(PassiveSlot slot)
    {
        explainName.text = slot.passiveSkill.Data.Name;
        sb.Clear();
        foreach ( var stat in slot.passiveSkill.PassiveEffect)
        {
            string stattype = null;
            if (stat.Type == PlayerStatType.STR) { stattype = "힘"; }
            else if (stat.Type == PlayerStatType.DEX) { stattype = "민첩"; }
            else if (stat.Type == PlayerStatType.CON) { stattype = "활력"; }
            else if (stat.Type == PlayerStatType.Armor) { stattype = "방어력"; }
            else if (stat.Type == PlayerStatType.CriRate) { stattype = "크리티컬 확률"; }
            else if (stat.Type == PlayerStatType.CriDamage) { stattype = "크리티컬 데미지"; }
            else if (stat.Type == PlayerStatType.MaxHp) { stattype = "최대체력"; }
            
            sb.Append(stattype).Append(" : ").Append(stat.StatValue).AppendLine();
        }
        sb.Append(slot.passiveSkill.Data.Description);
        explainDescription.text = sb.ToString();
    }
}
