using UnityEngine;

//[CreateAssetMenu(fileName = "BulletPattern", menuName = "Scriptable Objects/BulletPattern")]
public abstract class BulletPattern : ScriptableObject
{
    public Bullet patternBullet;
    public abstract void SpawnPattern(Transform _spawnPos);
}
