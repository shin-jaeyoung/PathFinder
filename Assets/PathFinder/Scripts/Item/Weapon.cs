using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon",menuName =("Item/Equipment/Weapon"))]
public class Weapon : Equipment
{
    protected override void UnUseEquipment(Player player)
    {
        //TODO : 무기칸제거, 인벤토리에 추가
        //근데 우클릭이든 뭐든 안하고 그냥 드래그드랍으로 처리할듯?
        //나중에 우클릭으로 아이템 사용하는거 만들거면 여기 해야함
    }

    protected override void UseEquipment(Player player, int index)
    {
        //TODO : 무기칸추가, 인벤토리에 제거
    }
}
