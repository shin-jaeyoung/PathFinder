using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    Menu,
    KeyGuide,
    Option,
    Exit,
    Inventory,
    Status,
    Skill,
    HUD,
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
    private UIType currenUItype;
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
            if(currenUItype == UIType.HUD)
            {
                ShowUI(UIType.Menu);
            }
            else
            {
                Showonly(UIType.HUD);
            }
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            Showonly(UIType.Inventory);
        }
        if(Input.GetKeyDown (KeyCode.U))
        {
            Showonly(UIType.Status);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            Showonly(UIType.Skill);
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
                if (data.type != UIType.HUD) panelGo.SetActive(false);
            }
        }
        //나중엔 GameStart넣을듯?
        currenUItype = UIType.HUD;
    }
    public void ShowUI(UIType type)
    {
        if (uiPanelDic.TryGetValue(type, out GameObject ui))
        {
            ui.SetActive(true);
            currenUItype = type;
        }
    }
    public void Showonly(UIType targetType)
    {
        foreach(var t in uiPanelDic)
        {
            
            if(t.Key==targetType)
            {
                t.Value.SetActive(true);
                currenUItype = targetType;
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
