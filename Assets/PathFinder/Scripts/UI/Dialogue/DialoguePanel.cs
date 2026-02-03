using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour, IPoolable
{
    [SerializeField]
    private int poolID;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private float heightOffset;

    private Camera mainCam;
    private Transform targetNpc;
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetID()
    {
        return poolID;
    }
    public void Init(string dialogueText, Transform targetNpc)
    {
        this.dialogueText.text = dialogueText;
        this.targetNpc = targetNpc;
        mainCam = Camera.main;
        transform.localScale = Vector3.one;
        StartCoroutine(SetPos(mainCam, targetNpc));
    }
    private void OnEnable()
    {
        StopAllCoroutines();
    }
    private void OnDisable()
    {
        targetNpc = null;
    }
    private IEnumerator SetPos(Camera mainCam, Transform targetNpc)
    {

        while (true)
        {
            if (mainCam != null && targetNpc != null)
            {
                Vector3 worldPos = targetNpc.position + new Vector3(0, heightOffset, 0);
                transform.position = mainCam.WorldToScreenPoint(worldPos);
            }
            yield return null;

        }
    }
}
