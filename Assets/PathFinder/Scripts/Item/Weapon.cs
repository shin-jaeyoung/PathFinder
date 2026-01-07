using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon",menuName =("Item/Equipment/Weapon"))]
public class Weapon : Equipment
{
    protected override void UnUseEquipment(Player player)
    {
        //TODO : 무기칸제거, 인벤토리에 추가
    }

    protected override void UseEquipment(Player player, int index)
    {
        //TODO : 무기칸추가, 인벤토리에 제거
    }
}
