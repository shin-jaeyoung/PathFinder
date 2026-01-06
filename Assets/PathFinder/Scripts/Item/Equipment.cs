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
    private List<EquipmentStatData> statDatas;

    //property
    public List<EquipmentStatData> StatData => statDatas;

    public void Equip()
    {
        UseEquipment();
    }
    public void UnEquip()
    {
        UnUseEquipment();
    }
    protected abstract void UseEquipment();
    protected abstract void UnUseEquipment();
}
