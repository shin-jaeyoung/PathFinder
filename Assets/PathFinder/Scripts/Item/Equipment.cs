using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[System.Serializable]
public struct EquipmentStatData
{
    [SerializeField]
    private PlayerStatType type;
    [SerializeField]
    private int statValue;

    //property
    public PlayerStatType Type => type;
    public int StatValue => statValue;
}
public abstract class Equipment : Item, IEquipable
{
    [SerializeField]
    protected List<EquipmentStatData> statDatas;
    [SerializeField]
    protected EquipmentType type;

    private StringBuilder sb = new StringBuilder();
    //property
    public List<EquipmentStatData> StatData => statDatas;
    public EquipmentType Type => type;

    public string Explain()
    {
        sb.Clear();
        foreach( EquipmentStatData statData in statDatas)
        {
            if (statData.Type == PlayerStatType.STR)
            {
                sb.Append($"힘 + {statData.StatValue}").AppendLine();
            }
            else if( statData.Type == PlayerStatType.DEX)
            {
                sb.Append($"민첩 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.CON)
            {
                sb.Append($"활력 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.Power)
            {
                sb.Append($"공격력 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.Armor)
            {
                sb.Append($"방어도 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.CriRate)
            {
                sb.Append($"치명타율 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.CriDamage)
            {
                sb.Append($"치명타 데미지 + {statData.StatValue}").AppendLine();
            }
            else if (statData.Type == PlayerStatType.MaxHp)
            {
                sb.Append($"최대체력 + {statData.StatValue}").AppendLine();
            }
        }
        sb.Append(Data.Description);

        return sb.ToString();
    }

    public void Equip(Player player, int index)
    {
        UseEquipment(player, index);
    }
    public void UnEquip(Player player)
    {
        UnUseEquipment(player );
    }
    protected abstract void UseEquipment(Player player, int index);
    protected abstract void UnUseEquipment(Player player);
}
