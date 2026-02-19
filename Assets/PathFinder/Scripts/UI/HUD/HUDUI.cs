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
    private Image expImage;
    [SerializeField]
    private TextMeshProUGUI expText;
    [Header("Hp")]
    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private TextMeshProUGUI hpText;
    [Header("Skill")]
    [SerializeField]
    private List<Image> skillList;
    [Header("HpPotion")]
    [SerializeField]
    private Image hpPotion;
    [SerializeField]
    private TextMeshProUGUI potionCount;




    private Player player;


    private void Init()
    {
        if(GameManager.instance.Player!=null)
        {
            player = GameManager.instance.Player;

            if (player.LevelSystem == null || player.StatusSystem == null ||
                player.Skills == null || player.Potion == null)
            {
                Debug.LogWarning("HUDUI: 플레이어의 서브 시스템이 아직 초기화되지 않았습니다. 재시도합니다.");
                Invoke(nameof(Init), 0.1f); 
                return;
            }

            player.LevelSystem.OnExpChanged += UpdateLevel;
            player.StatusSystem.OnStatChanged += UpdateHp;
            player.Skills.OnChangedActiveSkill += UpdateSkillUI;
            player.Potion.OnChanged += UpdatePotionUI;
            SkillManager.instance.OnCooltimeReduced += UpdateSkillUI;
            UpdateLevel();
            UpdateHp();
            UpdateSkillUI();
            UpdatePotionUI();
        }
    }
    private void Start()
    {
        StartCoroutine(WaitPlayer());
    }
    private IEnumerator WaitPlayer()
    {
        while(GameManager.instance.Player == null)
        {
            yield return null;
        }
        Init();
    }
    private void OnDestroy()
    {
        if (GameManager.instance.Player != null)
        {
            player.LevelSystem.OnExpChanged -= UpdateLevel;
            player.StatusSystem.OnStatChanged -= UpdateHp;
            player.Skills.OnChangedActiveSkill -= UpdateSkillUI;
            SkillManager.instance.OnCooltimeReduced -= UpdateSkillUI;
            player.Potion.OnChanged -= UpdatePotionUI;
        }
    }
    public void UpdateLevel()
    {
        if (GameManager.instance.Player == null) return;
        levelValue.text = player.LevelSystem.Level.ToString();
        float curExp = player.LevelSystem.CurExp;
        float maxExp = player.LevelSystem.MaxExp;
        float ratio = curExp / maxExp;
        expImage.fillAmount = ratio;
        expText.text = $"{curExp} / {maxExp}";
    }

    public void UpdateHp()
    {
        if (GameManager.instance.Player == null) return;
        hpImage.fillAmount = player.StatusSystem.Stat[PlayerStatType.CurHp] / player.StatusSystem.FinalStat[PlayerStatType.MaxHp];
        hpText.text = $"{player.StatusSystem.Stat[PlayerStatType.CurHp]} / {player.StatusSystem.FinalStat[PlayerStatType.MaxHp]}";
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
                var equipSkill = player.Skills.Skillequip[i];
                if (equipSkill.isCooltime)
                {
                    skillList[i].type = Image.Type.Filled;
                    skillList[i].fillMethod = Image.FillMethod.Radial360;
                    skillList[i].fillClockwise = false;
                    skillList[i].fillAmount = 1 - equipSkill.currentCooltime / equipSkill.skill.Data.Cooltime;
                }
            }
        }
    }
    public void UpdatePotionUI()
    {
        hpPotion.color = player.Potion.IsCoolTime ?
                        new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
        potionCount.text = player.Potion.CurCount.ToString();
    }
}
