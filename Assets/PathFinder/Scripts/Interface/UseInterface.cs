using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템
public interface IEquipable
{
    public void Equip(Player player, int index);
    public void UnEquip(Player player);
}

public interface IUseable
{
    public void Use();
}
public interface ISellable
{
    public int GetPrice();
}
public interface IMultiSellable :ISellable
{
    public int GetTotalPrice(int count);
}
//NPC
public interface IInteractable
{
    public void Interact(Player player);
}
public interface ISpecialInteractable
{
    public void SpecialInteract();
}
//Player

public interface ISkillActive
{
    public void Active();
}
//Monster


//공통
public interface IHittable
{
    public void Hit(DamageInfo info);
}