using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType
{
    Dialogue,
    Shop,
    
}
public class Npc : MonoBehaviour ,IInteractable
{
    [Header("NpcData")]
    [SerializeField]
    private NpcSO data;
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
                npc.SetNpc(this);
                specialNpcs.Add(npc);
            }
            if(type == NpcType.Shop)
            {
                ShopNpc npc = GetComponent<ShopNpc>();
                npc.SetNpc(this);
                specialNpcs.Add(npc);
            }
            
        }
    }

    public void Interact(Player player)
    {
        if(specialNpcs.Count>0)
        {
            
            if (specialNpcs[curNpcIndex].isInteractFinish)
            {
                specialNpcs[curNpcIndex].isInteractFinish = false;
                curNpcIndex++;
                if (curNpcIndex >= specialNpcs.Count)
                {
                    curNpcIndex = 0;
                }
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
