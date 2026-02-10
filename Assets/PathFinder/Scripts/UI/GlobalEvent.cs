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
    public static Action<string, Transform> OnDamage;
    public static void PrintDamage(string damage, Transform target )
    {
        OnDamage?.Invoke(damage, target);
    }

    //Dialogue을 Npc머리위에 출력하기 (대화내용, Npc Transform)
    public static Action<string, Transform> OnDialogue;
    public static void PrintDialogue(string dialogue, Transform targetNpc )
    {
        OnDialogue?.Invoke(dialogue, targetNpc);
    }
    public static Action OnDialogueEnd;

    public static Action<Monster> OnEncountBoss;
    public static void EncountBoss(Monster monster)
    {
        OnEncountBoss?.Invoke(monster);
    }

}
