using UnityEngine;

//[CreateAssetMenu(fileName = "BulletPattern", menuName = "Scriptable Objects/BulletPattern")]
public abstract class BulletPattern : ScriptableObject
{
    public Bullet patternBullet;
    public abstract void SpawnPattern(Transform _spawnPos, float _initialForce, int _polarity);

    public void FireBullet(Vector3 _spawnPos, Vector3 _newBulletDir, float _initialForce, int _polarity)
    {
        Bullet _newBullet = Instantiate(patternBullet, _spawnPos, Quaternion.identity);
        _newBullet.rb.AddForce(_newBulletDir * _initialForce);
        _newBullet.magnet.SetPolarity(_polarity);

    }
}
