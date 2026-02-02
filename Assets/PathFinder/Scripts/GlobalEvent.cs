using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class GlobalEvents
{
    // 알림을 보낼 때 사용할 이벤트 (내용, 지속시간)
    public static Action<string, float> OnNotify;

    public static void Notify(string message, float duration = 4f)
    {
        OnNotify?.Invoke(message, duration);
    }
    // 알림 보낼 때 사용할 이벤트 (데미지, 위치)
    public static Action<string, Vector2> OnDamage;
    public static void PrintDamage(string damage, Vector2 pos )
    {
        OnDamage?.Invoke(damage, pos);
    }
}
