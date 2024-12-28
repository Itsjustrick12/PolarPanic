using UnityEngine;

[CreateAssetMenu(fileName = "CircleBulletPattern", menuName = "Scriptable Objects/CircleBulletPattern")]
public class CircleBulletPattern : BulletPattern
{
    [SerializeField] public int numBullets = 8;
    [Tooltip("Rotates the circle about the Z axis")]
    [SerializeField] public float rotOffset = 0f;
    [Tooltip("Use to keep all bullets from spawning directly on center")]
    [SerializeField] public float posOffset = 1f;

    public override void SpawnPattern(Transform _spawnPos, float _initialForce, int _polarity)
    {
        //How much space between each shot in radians
        float _between = 360f / numBullets;

        for (int i = 0; i < numBullets; i++)
        {
            Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, i * _between + rotOffset) * Vector3.right;
            Bullet _newBullet = Instantiate(patternBullet, _spawnPos.position + posOffset * _newBulletDir, Quaternion.identity);
            FireBullet(_spawnPos.position + posOffset * _newBulletDir, _newBulletDir, _initialForce, _polarity);
        }
    }
}
