using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MagnetizedObj))]
public class Bullet : MonoBehaviour
{
    [SerializeField] public Collider2D physicsBody;
    public Rigidbody2D rb;
    public MagnetizedObj magnet;
    public SpriteRenderer sprite;
    public float damage = 1f;
    public bool reflected = false;

    public Sprite blue;
    public Sprite red;
    public Sprite white;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] public Gradient redTrail;
    [SerializeField] public Gradient blueTrail;
    [SerializeField] private GameObject redDeathEffect, blueDeathEffect;

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;

        if(magnet == null)
        {
            magnet = GetComponent<MagnetizedObj>();
        }
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        SetPolarity(magnet.GetPolarity());
    }

    public void SetPolarity(int polarity)
    {
        if (polarity == 0)
        {
            //Set white
            sprite.sprite = white;
        }
        if (polarity > 0)
        {
            //Set red
            sprite.sprite = red;
            trailRenderer.colorGradient = redTrail;
        }
        else
        {
            //Set blue
            sprite.sprite = blue;
            trailRenderer.colorGradient = blueTrail;
        }

        magnet.SetPolarity(polarity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
                DestroyBullet();
            }
        }
        if (collision.gameObject.CompareTag("Enemy") && reflected)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyBullet();
            }
        }
        if (collision.gameObject.layer == 11)
        {
            DestroyBullet();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        if (magnet.GetPolarity() > 0)
            Instantiate(redDeathEffect, transform.position, transform.rotation);
        else if (magnet.GetPolarity() < 0)
            Instantiate(blueDeathEffect, transform.position, transform.rotation);

        magnet.OnDestroy?.Invoke(magnet);
        
        Destroy(gameObject);
    }
}
