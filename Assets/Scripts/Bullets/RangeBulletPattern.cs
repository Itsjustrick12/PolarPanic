using UnityEngine;

[CreateAssetMenu(fileName = "RangeBulletPattern", menuName = "Scriptable Objects/RangeBulletPattern")]
public class RangeBulletPattern : BulletPattern
{
    [SerializeField] public int numBullets = 8;
    [Tooltip("Positive and negative range in degrees")]
    [SerializeField] public float range = 20f;
    [Tooltip("Rotates the range about the Z axis in degrees")]
    [SerializeField] public float rotOffset = 0f;
    [Tooltip("Use to keep all bullets from spawning directly on center")]
    [SerializeField] public float posOffset = 1f;
    [Tooltip("Whether or not to use random spread")]
    [SerializeField] public bool randomSpread = false;
    [Tooltip("Whether or not to use the source's rotation to rotate the pattern")]
    [SerializeField] public bool useSourceRotation = true;
    float sourceZRot = 0f;


    public override void SpawnPattern(Transform _spawnPos, float _initialSpeed, int _polarity, Enemy _shooter)
    {
        if (useSourceRotation)
        {
            sourceZRot = _spawnPos.rotation.eulerAngles.z;
        }

        if (randomSpread)
        {
            float _midSpread = rotOffset + sourceZRot;

            for (int i = 0; i < numBullets; i++)
            {
                Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, _midSpread + Random.Range(-range, range)) * Vector3.right;
                FireBullet(_spawnPos.position + posOffset * _newBulletDir, _newBulletDir, _initialSpeed, _polarity);
            }
        }
        else
        {
            //Special case because we'd have a divide by 0 otherwise
            if (numBullets == 1)
            {
                float _midSpread = rotOffset + sourceZRot;
                Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, _midSpread) * Vector3.right;
                FireBullet(_spawnPos.position + posOffset * _newBulletDir, _newBulletDir, _initialSpeed, _polarity);
            }
            else
            {
                //How much space between each shot in radians
                float _between = (2f * range) / (numBullets - 1);

                for (int i = 0; i < numBullets; i++)
                {
                    Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, i * _between + (rotOffset - range) + sourceZRot) * Vector3.right;
                    FireBullet(_spawnPos.position + posOffset * _newBulletDir, _newBulletDir, _initialSpeed, _polarity);
                }
            }
        }
    }
}
