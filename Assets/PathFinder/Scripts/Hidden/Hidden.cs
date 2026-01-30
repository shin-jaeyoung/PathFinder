using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HiddenState
{
    Start,
    Progress,
    End,
}
[System.Serializable]
public class Hidden 
{
    
    [SerializeField]
    private HiddenData data;

    [SerializeField]
    private HiddenState curState;
    public int curStep;
    [SerializeField]
    private int maxStep;
    
    public Hidden(HiddenData data)
    {
        this.data = data;
        curState = HiddenState.Start;
        curState = 0;
        maxStep = data.conditions.Count;
    }

    //property
    public HiddenData Data => data;


    public bool VerifyCondition(int targetId, Player player)
    {
        if (curState == HiddenState.End) return false;

        Condition currentCond = data.conditions[curStep];
        if (currentCond.targetId != targetId) return false;
        if (currentCond.conditionType == ConditionType.HaveItem)
        {
            if (!player.Inventory.HasItem(currentCond.needItem)) return false;
        }

        NextStep(player);
        return true;
    }


    public void NextStep(Player player)
    {
        curStep++;
        if (curStep >= data.conditions.Count)
        {
            curState = HiddenState.End;
            GiveReward(player);
        }
        else
        {
            curState = HiddenState.Progress;
        }
        Debug.Log($"{data.hiddenName} 진행도: {curStep}/{data.conditions.Count}");
    }
    private void GiveReward(Player player)
    {
        Debug.Log($"{data.hiddenName} 클리어! 보상을 지급합니다.");
        RewardManager.instance.Reward(data.rewardData, player.transform.position);
    }
}
