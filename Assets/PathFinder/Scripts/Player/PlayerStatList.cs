using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInitStat", menuName = "Player/InitStat")]
public class PlayerStatList : ScriptableObject
{
    [SerializeField]
    private List<PlayerStats> statsList;

    public List<PlayerStats> StatsList => statsList;
}
