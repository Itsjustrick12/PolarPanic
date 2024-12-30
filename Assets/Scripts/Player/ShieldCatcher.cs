using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldCatcher : MonoBehaviour
{
    [SerializeField] float vortexRange = 0.8f;
    [SerializeField] float vortexRotationSpeed = 225f;
    [SerializeField] public int maxBullets = 5;
    [SerializeField] public bool counterClockwise = true;
    [SerializeField] public float launchSpeed = 8f;
    [SerializeField] public Collider2D vortexCatcher;
    [SerializeField] ShieldController shieldController;
    [SerializeField] RawImage chargeFlash;
    [SerializeField] float chargeDisappearTime = 0.4f;
    [SerializeField] float chargeAmount = 1f;
    float chargeTime = 0f;
    public int polarity = 0;
    //public int curBullets = 0;
    public List<Bullet> bulletSlots = new();
    private float rotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < maxBullets; i++)
        {
            bulletSlots.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            chargeTime = chargeDisappearTime;
        }

        chargeTime -= Time.deltaTime;
        chargeFlash.color = new Color(185f/ 255f, 124f / 255f, 1f, chargeTime / chargeDisappearTime);
        //chargeFlash.color = new Color(1f, 216f / 255f, 119f / 255f, chargeTime / chargeDisappearTime);

        rotation += Time.deltaTime * vortexRotationSpeed;
        rotation %= 360f;

        if(shieldController.polarity != polarity)
        {
            LaunchHeldBullets();

            polarity = shieldController.polarity;
            counterClockwise = polarity == 1;
        }

        if(polarity == 0)
        {
            return;
        }

        float _between = 360f / bulletSlots.Count;
        float _rotDir = counterClockwise ? 1 : -1;
        for (int i = 0; i < bulletSlots.Count; i++)
        {
            //Skip missing
            if (bulletSlots[i] == null)
            {
                continue;
            }

            Vector3 _newBulletPos = Quaternion.Euler(0f, 0f, i * _between + rotation * _rotDir) * Vector3.right;
            bulletSlots[i].rb.MovePosition(transform.position + (_newBulletPos * vortexRange));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet _bullet))
        {
            if (bulletSlots.Contains(_bullet))
            {
                return;
            }

            _bullet.reflected = true;
            _bullet.physicsBody.excludeLayers = 0;

            if (polarity == 0 || polarity == _bullet.magnet.GetPolarity())
            {
                //Try to destroy bullet in vortex
                int _bulletToNeutralize = bulletSlots.FindIndex((x) => x != null);

                if (_bulletToNeutralize == -1)
                {
                    //No bullets, bounce away?
                }
                else
                {
                    //Bullets neutralize each other
                    bulletSlots[_bulletToNeutralize].DestroyBullet();
                    _bullet.DestroyBullet();
                }
            }
            else
            {
                //Catch bullet
                int _openSlot = bulletSlots.FindIndex(x => x == null);


                if(_openSlot == -1)
                {
                    //What do we do?
                }
                else
                {
                    //Ensure they destroy properly
                    _bullet.magnet.OnDestroy += RemoveBulletFromSlotsOnDestroy;

                    _bullet.rb.angularVelocity = 0f;
                    _bullet.rb.linearVelocity = Vector2.zero;

                    bulletSlots[_openSlot] = _bullet;
                }
            }
        }
    }

    public void LaunchHeldBullets()
    {
        float _between = 360f / bulletSlots.Count;
        float _rotDir = counterClockwise ? 1 : -1;
        for (int i = 0; i < bulletSlots.Count; i++)
        {
            //Skip missing
            if (bulletSlots[i] == null)
            {
                continue;
            }

            //Remove event
            bulletSlots[i].magnet.OnDestroy -= RemoveBulletFromSlotsOnDestroy;

            //Shoot off in direction of travel
            /*
            Vector3 _newBulletDir = Quaternion.Euler(0f, 0f, i * _between + (rotation + 90f) * _rotDir) * Vector3.right;
            bulletSlots[i].rb.linearVelocity = _newBulletDir * launchSpeed;
            */

            //Shoot of in direction of shield facing
            bulletSlots[i].rb.linearVelocity = shieldController.transform.right * launchSpeed;

            //Remove from list
            bulletSlots[i] = null;
        }
    }

    public void RemoveBulletFromSlotsOnDestroy(MagnetizedObj _destroyedMagnet)
    {
        int _removedIndex = bulletSlots.FindIndex(x => x != null && x.magnet == _destroyedMagnet);
        if(_removedIndex != -1)
        {
            bulletSlots[_removedIndex] = null;
        }
    }

    public void AddCharge()
    {

    }
}
