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
    float sourceZRot = 0f;


    public override void SpawnPattern(Transform _spawnPos)
    {
        if(numBullets <= 0)
        {
            return;
        }

        GameManager.instance.StartCoroutine(ShootAndWait(_spawnPos, numBullets));
    }

    public IEnumerator ShootAndWait(Transform _spawnPos, int _numBulletsRemaining)
    {
        Quaternion _newBulletDir = Quaternion.Euler(0f, 0f, rotOffset + _spawnPos.eulerAngles.z);
        Instantiate(patternBullet, _spawnPos.position + posOffset * (_newBulletDir * Vector3.right), _newBulletDir);
        _numBulletsRemaining--;

        //Stop shooting
        if(_numBulletsRemaining <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(timeBetweenShots);
        GameManager.instance.StartCoroutine(ShootAndWait(_spawnPos, _numBulletsRemaining));
    }
}
