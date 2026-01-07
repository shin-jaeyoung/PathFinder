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

//Player

public interface ISkillActive<T> where T :Player 
{
    public void Active(T caster);
}

