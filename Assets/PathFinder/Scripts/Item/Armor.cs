using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[CreateAssetMenu(fileName = "Armor", menuName = ("Item/Equipment/Armor"))]
public class Armor : Equipment
{
    protected override void UnUseEquipment(Player player)
    {
        if(type == EquipmentType.Helmet)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Helmet].IsEmpty()) return;
            //ToDO:장비칸에서 제거
            if (player.Inventory.AddItem((player.Inventory.Equipments[(int)EquipmentType.Helmet].item)))
            {
                Debug.Log("헬멧 해제");
            }
        }
        if(type == EquipmentType.Armor)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Armor].IsEmpty()) return;
            //ToDO:장비칸에서 제거
            if (player.Inventory.AddItem((player.Inventory.Equipments[(int)EquipmentType.Armor].item)))
            {
                Debug.Log("헬멧 해제");
            }
        }
        if (type == EquipmentType.Shoes)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Shoes].IsEmpty()) return;
            //ToDO:장비칸에서 제거
            if (player.Inventory.AddItem((player.Inventory.Equipments[(int)EquipmentType.Shoes].item)))
            {
                Debug.Log("헬멧 해제");
            }
        }
    }
 

    protected override void UseEquipment(Player player , int index)
    {
        if (type == EquipmentType.Helmet)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Helmet].IsEmpty())
            {
                player.Inventory.RemoveItem(player.Inventory.Inventory[index]);
                //TODO장비칸에 추가
            }
            else
            {
                //TODO:스왑로직만들어야함
                //여기서 할필요가 없네
            }
        }
        if (type == EquipmentType.Armor)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Armor].IsEmpty())
            {
                player.Inventory.RemoveItem(player.Inventory.Inventory[index]);
                //TODO장비칸에 추가
            }
            else
            {
                //TODO:스왑로직만들어야함
            }
        }
        if (type == EquipmentType.Shoes)
        {
            if (player.Inventory.Equipments[(int)EquipmentType.Shoes].IsEmpty())
            {
                player.Inventory.RemoveItem(player.Inventory.Inventory[index]);
                //TODO장비칸에 추가
            }
            else
            {
                //TODO:스왑로직만들어야함
            }
        }
    }
}
