using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackPattern", menuName = "Scriptable Objects/AttackPattern")]
public class AttackPattern : ScriptableObject
{
    [SerializeField] public List<AttackPatternData> attackPattern;
    [SerializeField] public bool randomOrder = false;
    [Tooltip("Only used when randomOrder is true, and there is more than one bullet pattern")]
    [SerializeField] public bool guaranteeNoRepeats = true;

    public float FireAttackPattern(ref int _patternPosition, Enemy _shooter, Transform _bulletSpawnTransform, float _initialForce, int _polarity)
    {
        AttackPatternData _newPattern;

        if (randomOrder)
        {
            
            int _newPatternIndex = Random.Range(0, attackPattern.Count);

            //Can't guarantee no repeats when there's only 1 attack
            if (guaranteeNoRepeats && attackPattern.Count > 1 && _newPatternIndex == _patternPosition)
            {
                //If we get the same number, increment by one and wrap over the list if necessary
                _newPatternIndex++;
                _newPatternIndex %= attackPattern.Count;
            }

            _newPattern = attackPattern[_newPatternIndex];
            _patternPosition = _newPatternIndex;
        }
        else
        {
            _newPattern = attackPattern[_patternPosition];
            _patternPosition++;
            _patternPosition %= attackPattern.Count;
        }

        _newPattern.pattern.SpawnPattern(_bulletSpawnTransform, _initialForce, _polarity, _shooter);
        return _newPattern.cooldown;
    }
}
