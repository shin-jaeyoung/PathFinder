using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Armor", menuName = ("Item/Equipment/Armor"))]
public class Armor : Equipment
{
    [SerializeField]
    private ArmorType type;

    //property
    public ArmorType Type => type;

    protected override void UnUseEquipment()
    {
        //TODO : 타입별 장착해제부위 다르게
    }

    protected override void UseEquipment()
    {
        //TODO : 타입별 장착부위 다르게
    }
}
