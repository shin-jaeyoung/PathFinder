using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템
public interface IEquipable
{
    public void Equip();
    public void UnEquip();
}

public interface IUseable
{
    public void Use();
}
//NPC
public interface IInteractable
{
    public void Interact(Player player);
}