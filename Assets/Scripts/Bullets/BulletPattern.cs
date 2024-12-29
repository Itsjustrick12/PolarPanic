using UnityEngine;

//[CreateAssetMenu(fileName = "BulletPattern", menuName = "Scriptable Objects/BulletPattern")]
public abstract class BulletPattern : ScriptableObject
{
    public Bullet patternBullet;
    public abstract void SpawnPattern(Transform _spawnPos, float _initialSpeed, int _polarity, Enemy _shooter);

    public void FireBullet(Vector3 _spawnPos, Vector3 _newBulletDir, float _initialForce, int _polarity)
    {
        Bullet _newBullet = Instantiate(patternBullet, _spawnPos, Quaternion.identity);
        _newBullet.rb.linearVelocity = _newBulletDir * _initialForce;
        _newBullet.SetPolarity(_polarity);
    }
}
