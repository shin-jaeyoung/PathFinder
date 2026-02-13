using UnityEngine;

public class BossPortalDoor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxcollider;
    [SerializeField] private int targetId = 99;

    private void Start()
    {
        if (HiddenManager.instance != null && HiddenManager.instance.HiddenDic.ContainsKey(targetId))
        {
            if (HiddenManager.instance.HiddenDic[targetId].State == HiddenState.End)
            {
                OpenDoor();
                return;
            }
        }

        HiddenManager.instance.OnHiddenStateChanged += HandleHiddenChanged;
    }

    private void HandleHiddenChanged(int id, HiddenState state)
    {
        if (id == targetId && state == HiddenState.End)
        {
            OpenDoor();
            HiddenManager.instance.OnHiddenStateChanged -= HandleHiddenChanged;
        }
    }

    private void OpenDoor()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if (HiddenManager.instance != null)
        {
            HiddenManager.instance.OnHiddenStateChanged -= HandleHiddenChanged;
        }
    }
}