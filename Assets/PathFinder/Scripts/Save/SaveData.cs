using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{

    public int level;
    public int curExp;
    public int maxExp;
    public int levelPoint;
    public int gold;
    public bool hasDashSkill;

    public List<StatSaveEntry> baseStats = new List<StatSaveEntry>();

    public List<int> invItemIDs = new List<int>();
    public List<int> invCounts = new List<int>();
    public List<int> equipItemIDs = new List<int>();

    public List<int> ownedActiveIDs = new List<int>();
    public List<int> ownedPassiveIDs = new List<int>();
    public List<int> quickSlotIDs = new List<int>();

    //매니저관련

    //포탈등록정보
    public List<PortalData> savedPortals = new List<PortalData>();
    // Hidden 시스템 저장용
    public List<HiddenSaveEntry> hiddenStates = new List<HiddenSaveEntry>();
    public int endHiddenCount;
}

[Serializable]
public struct StatSaveEntry
{
    public PlayerStatType statType;
    public float value;
}
[Serializable]
public struct HiddenSaveEntry
{
    public int id;
    public HiddenState state;
}
