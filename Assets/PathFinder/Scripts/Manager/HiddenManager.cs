using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenManager : MonoBehaviour
{
    public static HiddenManager instance;
    //SO로 만든 데이터 넣는곳
    [SerializeField]
    private List<HiddenData> datas;

    //인스펙터 확인용
    //datas로 Hidden을 만들어야함
    [SerializeField]
    private List<Hidden> hiddens;
    //실제쓰는 데이터 
    private Dictionary<int, Hidden> hiddenDic = new Dictionary<int, Hidden>();
    //플레이어 캐싱
    private Player player;

    private int endHiddenCount;
    //property
    public Dictionary<int, Hidden> HiddenDic => hiddenDic;
    public int EndHiddenCount
    {
        get {  return endHiddenCount; }
        set
        {
            endHiddenCount = value;
            OnEndHidden?.Invoke();
        }
    }
    //deligate
    public event Action OnEndHidden;
    public Action<int, HiddenState> OnHiddenStateChanged;


    private void Awake()
    {
        if(instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
            endHiddenCount = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        foreach (var data in datas)
        {
            Hidden newHidden = new Hidden(data);
            hiddens.Add(newHidden);
            hiddenDic.Add(data.id, newHidden);
        }
    }
    public void ProcessHidden(int hiddenId, int objId,Player player)
    {
        if (hiddenDic.TryGetValue(hiddenId, out Hidden hidden))
        {
            bool success = hidden.VerifyCondition(objId, player);
            if (success)
            {
                if(hidden.State == HiddenState.Progress)
                {
                    Debug.Log($"{hiddenId}번 기믹 통과");
                    //연출넣으면 될듯?
                }
            }
        }
    }
    public int CheckEndHiddenCount()
    {
        int count = 0;
        foreach ( var hidden in hiddenDic)
        {
            if(hidden.Value.State == HiddenState.End)
            {
                count++;
            }
        }
        return count;
    }
    public int ChechkHiddenCount()
    {
        return hiddenDic.Count;
    }
    public void CompleteHidden(int id)
    {
        if (HiddenDic.ContainsKey(id))
        {
            OnHiddenStateChanged?.Invoke(id, HiddenState.End);
        }
    }
}
