using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : MonoBehaviour, IInteractable
{
    //상점 데이터

    public void Interact(Player player)
    {
        //상점매니저에 데이터 전송
        //여기에 대화가 끝나면 상점열기도 가능 델리게이트에 넣어서
        //매니저는 데이터로 상점 리빌딩
        //그다음 상점 열기
        Debug.Log("상점오픈");
        UIManager.Instance.Showonly(UIType.Shop);
    }
}
