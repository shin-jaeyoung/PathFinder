using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    private int UIid;

    private Transform targetNpc;

    //기존 대화내용을 풀로 보내고 새 대화를 저장하기 위해 사용한 자료구조
    private Queue<DialoguePanel> dialogueQueue = new Queue<DialoguePanel>();

    private void OnEnable()
    {
        GlobalEvents.OnDialogue -= SpawnDialogue;
        GlobalEvents.OnDialogue += SpawnDialogue;

        GlobalEvents.OnDialogueEnd -= Clear;
        GlobalEvents.OnDialogueEnd += Clear;
    }

    private void OnDisable()
    {
        GlobalEvents.OnDialogue -= SpawnDialogue;
        GlobalEvents.OnDialogueEnd -= Clear;
    }

    private void SpawnDialogue(string dialogue, Transform targetNpc)
    {
        GameObject go = PoolManager.instance.PoolDic[PoolType.Dialogue].Pop(UIid, Vector3.zero, Quaternion.identity);

        go.transform.SetParent(transform, false);
        if(go.TryGetComponent<DialoguePanel>(out DialoguePanel panel))
        {
            dialogueQueue.Enqueue(panel);
            if (dialogueQueue.Count > 1)
            {
                DialoguePanel predialogue = dialogueQueue.Dequeue();
                PoolManager.instance.PoolDic[PoolType.Dialogue].ReturnPool(predialogue);
            }
            panel.Init(dialogue, targetNpc);
        }
    }
    public void Clear()
    {
        if (dialogueQueue.Count > 0)
        {
            DialoguePanel predialogue = dialogueQueue.Dequeue();
            PoolManager.instance.PoolDic[PoolType.Dialogue].ReturnPool(predialogue);
        }
        dialogueQueue.Clear();
    }
}
