using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackPattern", menuName = "Scriptable Objects/AttackPattern")]
public class AttackPattern : ScriptableObject
{
    [SerializeField] public List<AttackPatternData> attackPattern;
    [SerializeField] public bool randomOrder = false;
    [Tooltip("Only used when randomOrder is true, and there is more than one bullet pattern")]
    [SerializeField] public bool guaranteeNoRepeats = true;
}
