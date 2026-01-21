using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    Slash,
    Shoot,
    Magic,
}
[System.Serializable]
public struct SkillData 
{
    [SerializeField]
    private string skillName;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private SkillType skillType;
    [SerializeField]
    [TextArea(4,10)]
    private string description;
    [Header("Value")]
    [SerializeField]
    private float cooltime;
    [SerializeField]
    private float damageMultiplier;
    [SerializeField]
    private float duration;
    [Header("Detail")]
    [SerializeField]
    private float spriteRotation;
    //property
    public string SkillName => skillName;
    public GameObject Prefab => prefab;
    public Sprite Icon => icon;
    public SkillType Type => skillType;
    public string Description => description;
    public float Cooltime => cooltime;
    public float DamageMultiplier => damageMultiplier;
    public float Duration => duration;
    public float SpriteRotation => spriteRotation;

}
