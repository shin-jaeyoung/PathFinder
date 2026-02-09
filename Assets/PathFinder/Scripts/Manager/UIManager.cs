using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    GameStart,
    Menu,
    KeyGuide,
    Option,
    Exit,
    Inventory,
    Status,
    Skill,
    HUD,
    Dialogue,
    Shop,
    Portal,


    Off,
}
[System.Serializable]
public struct UIData
{
    public UIType type;
    public GameObject uiprefab;
}
public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    //리스트를 쓰는데 contains같은 순회연산하는 거 안하려고 만든 클래스
    private class UIStatus
    {
        public GameObject panel;
        public bool isOpen;
    }


    [Header("UI CanvasList")]
    [SerializeField]
    private List<GameObject> canvasList;

    [Header("UI Panel")]
    [SerializeField]
    private List<UIData> uiPanels;

    //내부에서 사용할 애들
    private Dictionary<UIType, UIStatus> uiPanelDic;
    private List<GameObject> instantiatedCanvases;
    //밑에스택으로 한번 해보자
    private UIType currenUIType;
    private UIType preUIType;

    //리스트를 스택처럼 써보자
    private List<UIType> uiStack = new List<UIType>();
    //property
    public UIType CurUI => currenUIType;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(WaitGM());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiStack.Count > 0)
            {
                HideUI(uiStack[uiStack.Count - 1]);
            }
            else
            {
                ShowUI(UIType.Menu);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleUI(UIType.Inventory);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUI(UIType.Status);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleUI(UIType.Skill);
        }
    }
    private void ToggleUI(UIType type)
    {
        if (!uiPanelDic.TryGetValue(type, out UIStatus status)) return;

        if (status.isOpen)
        {
            if (uiStack[uiStack.Count - 1] == type)
            {
                HideUI(type);
            }
            else
            {
                uiStack.Remove(type);
                uiStack.Add(type);
                status.panel.transform.SetAsLastSibling();
                UpdateCurrentUIType();
            }
        }
        else
        {
            ShowUI(type);
        }
    }
    private IEnumerator WaitGM()
    {
        while (GameManager.instance.Player == null)
        {
            yield return null;
        }
        Debug.Log("UI : GM기다리기 완료");
        Init();
    }
    private void Init()
    {
        uiPanelDic = new Dictionary<UIType, UIStatus>();
        instantiatedCanvases = new List<GameObject>();

        foreach (GameObject canvas in canvasList)
        {
            GameObject go = Instantiate(canvas, transform);
            instantiatedCanvases.Add(go);
        }

        foreach (var data in uiPanels)
        {
            if (data.uiprefab == null) continue;

            // HUD는 1번 캔버스, 나머지는 0번 캔버스
            Transform targetCanvas = (data.type == UIType.HUD) ?
                instantiatedCanvases[1].transform : instantiatedCanvases[0].transform;

            GameObject panelGo = Instantiate(data.uiprefab, targetCanvas);

            UIStatus status = new UIStatus { panel = panelGo, isOpen = false };
            uiPanelDic.Add(data.type, status);

            if (data.type != UIType.HUD) panelGo.SetActive(false);
        }

        // 초기 HUD 설정
        uiPanelDic[UIType.HUD].panel.SetActive(true);
        currenUIType = UIType.HUD;
    }
    public bool CheckCurUIType(UIType type)
    {
        if(currenUIType == type) return true;
        return false;
    }


    public void ShowUI(UIType type)
    {
        if (!uiPanelDic.TryGetValue(type, out UIStatus status)) return;
        if (status.isOpen) return; 
        status.panel.SetActive(true);
        status.panel.transform.SetAsLastSibling();

        status.isOpen = true;
        uiStack.Add(type);
        UpdateCurrentUIType();
    }
    public void HideUI(UIType type)
    {
        if (!uiPanelDic.TryGetValue(type, out UIStatus status)) return;
        if (!status.isOpen) return;

        status.panel.SetActive(false);
        status.isOpen = false;
        uiStack.Remove(type);

        UpdateCurrentUIType();
    }
    private void UpdateCurrentUIType()
    {
        currenUIType = (uiStack.Count > 0) ? 
            uiStack[uiStack.Count - 1] : UIType.HUD;

    }
    public void Showonly(UIType targetType)
    {
        foreach (var pair in uiPanelDic)
        {
            if (pair.Key == UIType.HUD) continue;

            if (pair.Key == targetType)
            {
                pair.Value.panel.SetActive(true);
                pair.Value.panel.transform.SetAsLastSibling();
                pair.Value.isOpen = true;
            }
            else
            {
                pair.Value.panel.SetActive(false);
                pair.Value.isOpen = false;
            }
        }

        uiStack.Clear();
        uiStack.Add(targetType);
        UpdateCurrentUIType();
    }
}
