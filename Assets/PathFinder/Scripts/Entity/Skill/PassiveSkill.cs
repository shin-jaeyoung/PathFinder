using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PassiveSkillData
{
    [SerializeField]
    private string skillName;
    [SerializeField]
    private string description;
    [SerializeField]
    private Sprite icon;

    //property
    public string Name => skillName;
    public string Description => description;
    public Sprite Icon => icon;
}

[CreateAssetMenu (fileName ="PassiveSkill", menuName =("Skill/PassiveSkill"))]
public class PassiveSkill : ScriptableObject
{
    [Header("Passive StatList")]
    [SerializeField]
    private List<EquipmentStatData> passiveEffect;
    [Header("Data")]
    [SerializeField]
    private PassiveSkillData data;

    public List<EquipmentStatData> PassiveEffect => passiveEffect;
    public PassiveSkillData Data => data;
}
