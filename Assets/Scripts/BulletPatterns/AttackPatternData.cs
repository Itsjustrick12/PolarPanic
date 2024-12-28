using UnityEngine;

public class AttackPatternData
{
    [Tooltip("The bullet pattern to be used")]
    [SerializeField] public BulletPattern pattern;
    [Tooltip("How long the enemy has to wait to fire the next pattern")]
    [SerializeField] public float timeout = 0f;
}
