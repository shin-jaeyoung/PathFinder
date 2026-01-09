using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIButtonData
{
    public UIType type;
    public Button button;
}
public class MenuUI : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField]
    private List<UIButtonData> buttons;

    void Start()
    {
        if(UIManager.Instance != null)
        {
            foreach (var button in buttons)
            {
                button.button.onClick.RemoveAllListeners();
                UIType type = button.type;
                button.button.onClick.AddListener(()=>UIManager.Instance.ShowUI(type));
                
            }
        }
    }
}
