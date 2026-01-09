using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour
{
    [Header("Level")]
    [SerializeField]
    private TextMeshProUGUI levelValue;
    [SerializeField]
    private Slider expSlider;
    [Header("Hp")]
    [SerializeField]
    private Slider hpSlider;
    [Header("Skill")]
    [SerializeField]
    private List<Image> skillList;

    private Player player;


    private void OnEnable()
    {
        if(GameManager.instance.Player!=null)
        {
            player = GameManager.instance.Player;
            player.LevelSystem.OnExpChanged += UpdateLevel;
            player.StatusSystem.OnStatChanged += UpdateHp;
            player.Skills.OnChangedActiveSkill += UpdateSkillUI;

            UpdateLevel();
            UpdateHp();
            UpdateSkillUI();
        }
    }
    private void OnDisable()
    {
        if (GameManager.instance.Player != null)
        {
            player.LevelSystem.OnExpChanged -= UpdateLevel;
            player.StatusSystem.OnStatChanged -= UpdateHp;
            player.Skills.OnChangedActiveSkill -= UpdateSkillUI;
        }
    }
    public void UpdateLevel()
    {
        if (GameManager.instance.Player == null) return;
        levelValue.text = player.LevelSystem.Level.ToString();
        expSlider.value = player.LevelSystem.CurExp / player.LevelSystem.MaxExp;
    }

    public void UpdateHp()
    {
        if (GameManager.instance.Player == null) return;
        hpSlider.value = player.StatusSystem.Stat[PlayerStatType.CurHp] / player.StatusSystem.Stat[PlayerStatType.MaxHp];
    }

    public void UpdateSkillUI()
    {
        if (GameManager.instance.Player == null) return;



        for(int i = 0; i<skillList.Count; i++)
        {
            if(player.Skills.Skillequip[i].IsEmpty())
            {
                skillList[i].sprite = null;
                skillList[i].color = new Color(1, 1, 1, 0);
            }
            else
            {
                skillList[i].sprite = player.Skills.Skillequip[i].skill.Data.Icon;
                skillList[i].color = new Color(1, 1, 1, 1);
            }
        }
    }
}
