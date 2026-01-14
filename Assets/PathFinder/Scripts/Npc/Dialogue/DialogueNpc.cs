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
        if(isTalk)
        {
            if (dialogueSO.dialogues.Count > curIndex)
            {
                NextDialogue();
            }
            else
            {
                EndDialogue();
            }
        }
    }
    public void StartDialogue()
    {
        //DialogueManager에 본인 정보 넘겨주기
        if (dialogueSO.dialogues[curIndex] != null)
        {
            isTalk = true;
            NextDialogue();
        }
    }
    public void NextDialogue()
    {
        curDialogueText = dialogueSO.dialogues[curIndex];
        curIndex++;
        
    }
    public void EndDialogue()
    {
        curIndex = 0;
        isTalk = false;
        //대화가 끝났음을 알려줘야함 Npc에게 그래야 다음 스페셜 액션이 실행되도록할 수 있을듯

        isInteractFinish = true;
    }
}
