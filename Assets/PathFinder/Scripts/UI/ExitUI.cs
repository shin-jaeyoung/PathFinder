using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    [Header("UI Button")]
    [SerializeField]
    private UIButtonData yesbutton;
    [SerializeField]
    private UIButtonData nobutton;
    void Start()
    {
        if(UIManager.Instance != null)
        {
            yesbutton.button.onClick.RemoveAllListeners();
            nobutton.button.onClick.RemoveAllListeners();

            yesbutton.button.onClick.AddListener(() => GameExit());
            UIType noButtonType = nobutton.type;
            nobutton.button.onClick.AddListener(() => UIManager.Instance.HideUI(noButtonType));
        }
    }

    private void GameExit()
    {
        UIManager.Instance.Showonly(UIType.HUD);
        //게임종료 로직
        Debug.Log("게임종료");
    }
}
