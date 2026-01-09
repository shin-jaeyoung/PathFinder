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
    private List<Image> equipSkill;

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
    private List<Image> activeSkillInven;
    private List<Image> passiveSkillInven;

    public void Init()
    {
        if(GameManager.instance.Player!=null)
        {
            player = GameManager.instance.Player;
            activeSkillInven = new List<Image>(player.Skills.ActiveCapacity);
            passiveSkillInven = new List<Image>(player.Skills.PassiveCapacity);

            for (int i = 0; i < player.Skills.ActiveCapacity; i++)
            {
                Image icon = activeContent.transform.GetChild(i).GetChild(0)?.GetComponent<Image>();
                activeSkillInven.Add(icon);
            }
            for(int i = 0; i <player.Skills.PassiveCapacity; i++)
            {
                Image icon = passiveContent.transform.GetChild(i).GetChild(0)?.GetComponent<Image>();
                passiveSkillInven.Add(icon);
            }
            explainName.text = "";
            explainDescription.text = "IF click Skill Icon, you can see the skill's description";
        }
    }
    private void Start()
    {
        Init();
        activeButton.onClick.RemoveAllListeners();
        passiveButton.onClick.RemoveAllListeners();
        activeButton.onClick.AddListener(() => SwichTab(true));
        passiveButton.onClick.AddListener(()=>SwichTab(false));
        player.Skills.OnChangedActiveSkill += UpdateActiveSkillInven;
        player.Skills.OnChangedPassiveSkill += UpdatePassiveSkillInven;
    }
    private void OnDestroy()
    {
        activeButton.onClick.RemoveAllListeners();
        passiveButton.onClick.RemoveAllListeners();
        player.Skills.OnChangedActiveSkill -= UpdateActiveSkillInven;
        player.Skills.OnChangedPassiveSkill -= UpdatePassiveSkillInven;
    }
    public void SwichTab(bool isActive)
    {
        activePanel.SetActive(isActive);
        passivePanel.SetActive(!isActive);
    }
    public void UpdateActiveSkillInven()
    {
        for (int i = 0; i < player.Skills.ActiveCapacity; i++)
        {
            if (player.Skills.ActiveSkills[i].IsEmpty() || player.Skills.ActiveSkills[i].skill.Data.Icon == null)
            {
                activeSkillInven[i].sprite = null;
                activeSkillInven[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                activeSkillInven[i].sprite = player.Skills.ActiveSkills[i].skill.Data.Icon;
                activeSkillInven[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
    public void UpdatePassiveSkillInven()
    {
        for (int i = 0; i < player.Skills.PassiveCapacity; i++)
        {
            if (player.Skills.PassiveSkills[i].IsEmpty() || player.Skills.PassiveSkills[i].passiveSkill.Data.Icon == null)
            {
                passiveSkillInven[i].sprite = null;
                passiveSkillInven[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                passiveSkillInven[i].sprite = player.Skills.PassiveSkills[i].passiveSkill.Data.Icon;
                passiveSkillInven[i].color = new Color(1, 1, 1, 1);
            }
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
