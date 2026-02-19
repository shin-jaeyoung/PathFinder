using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private ItemType type;
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
    public int ID => id;
    public Sprite Sprite => sprite;
    public ItemType Type => type;
    public string Name => name;
    public string Description => description;
    public int Price => price;
}

public abstract class Item : ScriptableObject ,ISellable
{
    [SerializeField]
    private ItemData data;
    
    public ItemData Data => data;

    public int GetPrice()
    {
        return data.Price;
    }
}