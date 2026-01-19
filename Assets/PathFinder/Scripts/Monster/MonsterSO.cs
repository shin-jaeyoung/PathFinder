using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Monster",menuName ="Monster/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    [SerializeField]
    private MonsterData data;

    public MonsterData Data => data;
}
