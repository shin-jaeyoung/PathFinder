using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SpecialNpc : MonoBehaviour, ISpecialInteractable
{
    public bool isInteractFinish = false;

    public abstract void SpecialInteract();
}
