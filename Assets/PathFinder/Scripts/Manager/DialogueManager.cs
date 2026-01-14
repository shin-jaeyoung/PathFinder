using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueManager instance;

    private Npc curNpc;
    private DialogueNpc curDialogueNpc;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetNpc(Npc npc,DialogueNpc dialogueNpc)
    {
        curNpc = npc;
        curDialogueNpc = dialogueNpc;
    }
    
}
