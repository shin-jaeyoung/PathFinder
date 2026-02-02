using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private Transform container; 
    [SerializeField] private int textID;

    private void OnEnable()
    {
        GlobalEvents.OnNotify -= SpawnNotification;
        GlobalEvents.OnNotify += SpawnNotification;
    }
    private void OnDestroy()
    {
        GlobalEvents.OnNotify -= SpawnNotification;
    }

    private void SpawnNotification(string message, float duration)
    {
        GameObject go = PoolManager.instance.PoolDic[PoolType.UI].Pop(textID, container.position, Quaternion.identity);
        go.transform.SetParent(container,false);

        if (go.TryGetComponent(out NotificationText notifyText))
        {
            notifyText.SetText(message, duration);
        }
    }
}