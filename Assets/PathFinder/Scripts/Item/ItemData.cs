using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string name;
    [SerializeField]
    [TextArea(4,10)]
    private string description;
    [SerializeField]
    private int price;

    //property
    public Sprite Sprite => sprite;
    public string Name => name;
    public string Description => description;
    public int Price => price;
}

public abstract class Item : ScriptableObject
{
    [SerializeField]
    private ItemData data;
    
}