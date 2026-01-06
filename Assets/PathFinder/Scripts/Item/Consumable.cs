using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item, IUseable
{
    [SerializeField]
    private float coolTime;
    public void Use()
    {
        Effect();
    }
    protected abstract void Effect();
}
