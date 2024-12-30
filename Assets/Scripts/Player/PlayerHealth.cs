using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 0;
    public int maxHealth = 10;
    public bool isDead = false;

    public bool invincible = false;
    public SoundClip hurt, heal, death;
    public SoundPlayer soundPlayer;
    [SerializeField] private GameObject deathEffect;
    public float maxInvincibility = 0.1f;
    private float startInvincibility;

    private void Start()
    {
        health = maxHealth;
    }

    public void Update()
    {
        if (invincible && !isDead && Time.time >= startInvincibility + maxInvincibility)
        {
            invincible = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if ( !invincible )
        {

            health -= damage;
            //Update heart UI
            GameManager.instance.UpdateHealth(health);

            GetComponent<SimpleFlash>().Flash(1, 3, true);
            if (health < 0f && !isDead)
            {
                soundPlayer.PlaySound(death);
                Die();
            }
            else
            {
                soundPlayer.PlaySound(hurt);
                startInvincibility = Time.time;
                invincible = true;
            }
        }
    }

    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        soundPlayer.PlaySound(heal);
    }

    public void Die()
    {
        isDead = true;
        Instantiate(deathEffect, transform.position, transform.rotation);
        GameManager.instance.GameOver();
    }
}
