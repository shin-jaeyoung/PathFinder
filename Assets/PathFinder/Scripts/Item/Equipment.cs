using System;
using System.Collections;
using System.Collections.Generic;
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
    //property
    public List<EquipmentStatData> StatData => statDatas;
    public EquipmentType Type => type;

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
