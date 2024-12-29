using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MagnetizedObj))]
public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public MagnetizedObj magnet;
    public SpriteRenderer sprite;
    public float damage = 1f;
    bool reflected = false;

    public Sprite blue;
    public Sprite red;
    public Sprite white;

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
            //Set blue
            sprite.sprite = white;
        }
        if (polarity > 0)
        {
            //Set red
            sprite.sprite = red;
        }
        else
        {
            sprite.sprite = blue;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyBullet();
            }
        }
        if (collision.collider.CompareTag("Wall"))
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        magnet.OnDestroy?.Invoke(magnet);
        Destroy(gameObject);
    }
}
