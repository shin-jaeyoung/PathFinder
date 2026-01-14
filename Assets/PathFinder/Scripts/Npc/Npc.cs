using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType
{
    Dialogue,
    Shop,
    Healer
}
public class Npc : MonoBehaviour ,IInteractable
{
    [Header("Npc's specialActions")]
    [SerializeField]
    private List<NpcType> npcTypes;


    private List<SpecialNpc> specialNpcs;
    private int curNpcIndex = 0;

    private void Start()
    {
        specialNpcs = new List<SpecialNpc>();
        foreach(NpcType type in npcTypes)
        {
            if(type == NpcType.Dialogue)
            {
                DialogueNpc npc = GetComponent<DialogueNpc>();
                specialNpcs.Add(npc);
            }
            if(type == NpcType.Shop)
            {
                ShopNpc npc = GetComponent<ShopNpc>();
                specialNpcs.Add(npc);
            }
            if(type == NpcType.Healer)
            {
                
            }
        }
    }

    public void Interact(Player player)
    {
        if(specialNpcs.Count>0)
        {
            if(curNpcIndex >= specialNpcs.Count)
            {
                curNpcIndex = 0;
            }
            if (specialNpcs[curNpcIndex].isInteractFinish)
            {
                specialNpcs[curNpcIndex].isInteractFinish = false;
                curNpcIndex++;
            }
            specialNpcs[curNpcIndex].SpecialInteract();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision!=null && collision.GetComponent<Player>()!=null)
        {
            curNpcIndex = 0;
        }
    }
}
