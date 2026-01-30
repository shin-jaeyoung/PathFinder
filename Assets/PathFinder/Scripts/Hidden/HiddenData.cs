using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hidden", menuName = "Hidden/Hidden")]
public class HiddenData : ScriptableObject
{
    public int id;
    public string hiddenName;
    public RewardData rewardData;
    public List<Condition> conditions;
}

public enum ConditionType
{
    HaveItem,
    TalkToTarget,
    KillMonster
}
[System.Serializable]
public struct Condition
{
    public ConditionType conditionType;
    public int targetId;
    public Item needItem;
}