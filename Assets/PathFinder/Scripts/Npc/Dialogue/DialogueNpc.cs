using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNpc : SpecialNpc, ISpecialInteractable
{
    public Npc npc;
    public DialogueSO dialogueSO;
    public string curDialogueText;
    private int curIndex = 0;
    public bool isTalk = false;

    public void SetNpc(Npc npc)
    {
        this.npc = npc;
    }
    public override void SpecialInteract()
    {
        if (!isTalk)
        {
            StartDialogue();
        }
        else
        {
            //DialogueUI만들고 넣자
            //if(!UIManager.Instance.CheckCurUIType(UIType.Dialogue))
            //{
            //    CancleAct();
            //}
            
            NextDialogue();
        }
    }
    public void StartDialogue()
    {
        if (dialogueSO.dialogues[curIndex] != null)
        {
            isTalk = true;
            Debug.Log(dialogueSO.dialogues.Count);
            NextDialogue();
        }
    }
    public void NextDialogue()
    {
        if(dialogueSO.dialogues.Count -1 < curIndex)
        {
            EndDialogue();
            return;
        }
        GlobalEvents.PrintDialogue(dialogueSO.dialogues[curIndex], transform);
        curIndex++;
    }
    
    public void EndDialogue()
    {
        curIndex = 0;
        isTalk = false;
        GlobalEvents.OnDialogueEnd?.Invoke();

        isInteractFinish = true;
    }
    public void CancleAct()
    {
        curIndex = 0;
        isTalk = false;
        GlobalEvents.OnDialogueEnd?.Invoke();
        isInteractFinish = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            CancleAct();
        }
    }
}
