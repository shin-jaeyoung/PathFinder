using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RewardData
{
    public List<Item> items;
    public Skill activeSkill;
    public PassiveSkill passiveSkill;
    public int gold;
    public int exp;
}
public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    [SerializeField]
    private GameObject itemDropPrefab;

    private Player player;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //장비,기타아이템 =>인벤소관
    //스킬 =>스킬소관
    //경험치 => 레벨시스템소관
    //돈 => 인벤소관
    //4가지함수를 만들기 귀찮은데
    //즉시 지급 or 드랍아이템 으로 나뉠듯
    private void Start()
    {
        player = GameManager.instance.Player;
    }
    //외부에서 사용할 함수
    public void Reward(RewardData data,Vector2 pos)
    {
        GiveRewardDirect(data);
        GenerateDropItem(data, pos);
    }
    //즉시지급
    private void GiveRewardDirect(RewardData data)
    {
        //나중에 HUD창에 알림Text하나 만들어서 뭐 얻었는지 뜨게 하는것도 좋을듯
        if(data.activeSkill != null)
        {
            player.Skills.AddActiveSkill(data.activeSkill);
        }
        if(data.passiveSkill != null)
        {
            player.Skills.AddPassiveSkill(data.passiveSkill);
        }
        if (data.gold > 0) 
        { 
            player.Inventory.AddGold(data.gold); 
        }
        if (data.exp > 0)
        { 
            player.LevelSystem.AddExp(data.exp); 
        }
    }
    //아이템 생성해서 먹게하기
    private void GenerateDropItem(RewardData data,Vector2 pos)
    {
        foreach (Item item in data.items)
        {
            if (item == null) continue;
            Vector2 scatterPos = pos + Random.insideUnitCircle * 1.5f;

            GameObject go = Instantiate(itemDropPrefab, scatterPos, Quaternion.identity);
            if(go.TryGetComponent<Reward>(out Reward reward))
            {
                reward.Init(item);
            }
        }
    }

}
