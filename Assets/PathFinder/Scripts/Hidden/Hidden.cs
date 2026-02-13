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
        curStep = 0;
        maxStep = data.conditions.Count;
    }

    //property
    public HiddenData Data => data;
    public HiddenState State => curState;


    public bool VerifyCondition(int targetId, Player player)
    {
        if (curState == HiddenState.End) return false;

        Condition currentCond = data.conditions[curStep];
        if (currentCond.targetId != targetId) return false;
        if (currentCond.conditionType == ConditionType.HaveItem)
        {
            if (!player.Inventory.CheckItemAndRemove(currentCond.needItem)) return false;
        }
        if( currentCond.conditionType == ConditionType.Level)
        {
            if(player.LevelSystem.Level < currentCond.count) return false;
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
            HiddenManager.instance.EndHiddenCount++;
            HiddenManager.instance.CompleteHidden(data.id);
            GiveReward(player);
        }
        else
        {
            curState = HiddenState.Progress;
        }
        if(curStep < data.conditions.Count)
        {
            Debug.Log($"{data.hiddenName} 진행도: {curStep}/{data.conditions.Count}");
            GlobalEvents.Notify($"{data.hiddenName} 진행도: {curStep}/{data.conditions.Count}", 8f);
        }
    }
    private void GiveReward(Player player)
    {
        //여기도 연출 넣으면 될듯
        Debug.Log($"{data.hiddenName} 클리어! 보상을 지급합니다.");
        GlobalEvents.Notify($"{data.hiddenName} 클리어! 보상을 지급합니다.", 8f);
        RewardManager.instance.Reward(data.rewardData, player.transform.position);
    }
}
