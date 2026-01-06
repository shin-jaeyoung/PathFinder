using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    public void Equip();
    public void UnEquip();
}

public interface IUseable
{
    public void Use();
}