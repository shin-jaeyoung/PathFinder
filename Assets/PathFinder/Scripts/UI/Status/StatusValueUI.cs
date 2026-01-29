using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusValueUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI strValue;
    [SerializeField]
    private TextMeshProUGUI dexValue;
    [SerializeField]
    private TextMeshProUGUI conValue;
    [SerializeField]
    private TextMeshProUGUI powerValue;
    [SerializeField]
    private TextMeshProUGUI defValue;
    [SerializeField]
    private TextMeshProUGUI criRateValue;
    [SerializeField]
    private TextMeshProUGUI criDmgValue;


    public void RefreshUI(PlayerStatusSystem stat)
    {
        strValue.text = stat.FinalStat[PlayerStatType.STR].ToString();
        dexValue.text = stat.FinalStat[PlayerStatType.DEX].ToString();
        conValue.text = stat.FinalStat[PlayerStatType.CON].ToString();
        powerValue.text = stat.FinalStat[PlayerStatType.Power].ToString();
        defValue.text = stat.FinalStat[PlayerStatType.Armor].ToString();
        criRateValue.text = stat.FinalStat[PlayerStatType.CriRate].ToString();
        criDmgValue.text = stat.FinalStat[PlayerStatType.CriDamage].ToString();
    }


}
