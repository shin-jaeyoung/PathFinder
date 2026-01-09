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
    [Header("UI CanvasList")]
    [SerializeField]
    private List<GameObject> canvasList;

    [Header("UI Panel")]
    [SerializeField]
    private List<UIData> uiPanels;

    //내부에서 사용할 애들
    private Dictionary<UIType, GameObject> uiPanelDic;
    private List<GameObject> instantiatedCanvases;
    //밑에스택으로 한번 해보자
    private UIType currenUIType;
    private UIType preUIType;

    private Stack<UIType> UIStack;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currenUIType == UIType.HUD)
            {
                ShowUI(UIType.Menu);
            }
            else
            {
                CheckCurUI();
                if (currenUIType == UIType.HUD) return;
                HideUI(currenUIType);
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (currenUIType == UIType.Inventory)
            {
                HideUI(UIType.Inventory);
            }
            else
            {
                Showonly(UIType.Inventory);
            }
        }
        if(Input.GetKeyDown (KeyCode.U))
        {
            if (currenUIType == UIType.Status)
            {
                HideUI(UIType.Status);
            }
            else
            {
                Showonly(UIType.Status);
            }
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (currenUIType == UIType.Skill)
            {
                HideUI(UIType.Skill);
            }
            else
            {
                Showonly(UIType.Skill);
            }
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
        uiPanelDic = new Dictionary<UIType, GameObject>();
        instantiatedCanvases = new List<GameObject>();

        foreach (GameObject canvas in canvasList)
        {
            GameObject go = Instantiate(canvas, transform);
            instantiatedCanvases.Add(go);
        }
        foreach (var data in uiPanels)
        {
            if (data.uiprefab == null) continue;

            Transform targetCanvas = (data.type == UIType.HUD) ?
                instantiatedCanvases[0].transform : instantiatedCanvases[1].transform;

            GameObject panelGo = Instantiate(data.uiprefab, targetCanvas);

            if (!uiPanelDic.ContainsKey(data.type))
            {
                uiPanelDic.Add(data.type, panelGo);
                //이게 초기설정인데 GameStart씬부터 시작한다고하면 바꿔야함
                if (data.type != UIType.HUD) panelGo.SetActive(false);
            }
        }
        //나중엔 GameStart넣을듯?
        UIStack = new Stack<UIType>();
        UIStack.Push(UIType.HUD);

        //preUIType = UIType.HUD;
        currenUIType = UIType.HUD;
    }
    private UIType CheckCurUI()
    {
        currenUIType = UIStack.Pop();
        UIStack.Push(currenUIType);

        return currenUIType;
    }
    public void ShowUI(UIType type)
    {
        if (uiPanelDic.TryGetValue(type, out GameObject ui))
        {
            ui.SetActive(true);
            //preUIType = currenUIType;
            currenUIType = type;
            UIStack.Push(type);
        }
    }
    public void HideUI(UIType type)
    {
        if(uiPanelDic.TryGetValue(type,out GameObject ui))
        {
            ui.SetActive(false);
            //currenUIType = preUIType;
            UIStack.Pop();
            CheckCurUI();
        }
    }
    public void Showonly(UIType targetType)
    {
        foreach(var t in uiPanelDic)
        {
            
            if(t.Key==targetType)
            {
                t.Value.SetActive(true);
                preUIType = currenUIType;
                currenUIType = targetType;

                UIStack.Clear();
                UIStack.Push(UIType.HUD);
                UIStack.Push(targetType);
                CheckCurUI();
            }
            else
            {
                t.Value.SetActive(false);
            }
            if (t.Key == UIType.HUD)
            {
                t.Value.SetActive(true);
            }
        }
    }
}
