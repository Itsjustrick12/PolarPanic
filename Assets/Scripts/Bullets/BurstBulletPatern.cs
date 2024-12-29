using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstBulletPattern", menuName = "Scriptable Objects/BurstBulletPattern")]
public class BurstBulletPattern : BulletPattern
{
    [SerializeField] public int numBullets = 8;
    [Tooltip("Time between each fired bullet in seconds")]
    public float timeBetweenShots = 1f;
    [Tooltip("Rotates the range about the Z axis in degrees")]
    [SerializeField] public float rotOffset = 0f;
    [Tooltip("Use to keep all bullets from spawning directly on center")]
    [SerializeField] public float posOffset = 1f;
    [Tooltip("Whether or not to use the source's rotation to rotate the pattern")]
    [SerializeField] public bool useSourceRotation = true;


    public override void SpawnPattern(Transform _spawnPos, float _initialSpeed, int _polarity, Enemy _shooter)
    {
        if(numBullets <= 0)
        {
            return;
        }

        //_shooter.activeBulletCoroutines.Add(_shooter.StartCoroutine(_newCoroutine));
        _shooter.StartCoroutine(ShootAndWait(_spawnPos, numBullets, _initialSpeed, _polarity, _shooter));
    }

    public IEnumerator ShootAndWait(Transform _spawnPos, int _numBulletsRemaining, float _initialForce, int _polarity, Enemy _shooter)
    {
        Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, rotOffset + (useSourceRotation ? _spawnPos.eulerAngles.z : 0f)) * Vector3.right;
        FireBullet(_spawnPos.position + posOffset * _newBulletDir, _newBulletDir, _initialForce, _polarity);

        _numBulletsRemaining--;

        //Stop shooting
        if(_numBulletsRemaining <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(timeBetweenShots);
        //_shooter.activeBulletCoroutines.Add(_shooter.StartCoroutine(ShootAndWait(_spawnPos, _numBulletsRemaining, _initialForce, _polarity, _shooter)));
        _shooter.StartCoroutine(ShootAndWait(_spawnPos, _numBulletsRemaining, _initialForce, _polarity, _shooter));
    }
}
