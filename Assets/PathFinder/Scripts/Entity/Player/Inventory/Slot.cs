using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Slot
{
    public int count;

    public abstract bool IsEmpty();

    public abstract void Clear();
    

}
