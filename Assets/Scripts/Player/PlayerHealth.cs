using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 0f;
    public float maxHealth = 3f;
    public bool isDead = false;

    public bool invincible = false;
    public SoundClip hurt, death;
    public SoundPlayer soundPlayer;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if ( !invincible )
        {
            
            health -= damage;
            GetComponent<SimpleFlash>().Flash(1, 3, true);
            if (health < 0f && !isDead)
            {
                soundPlayer.PlaySound(death);
                Die();
            }
            else
            {
                soundPlayer.PlaySound(hurt);
            }
        }
    }

    public void Die()
    {
        isDead = true;
        GameManager.instance.GameOver();
    }
}
