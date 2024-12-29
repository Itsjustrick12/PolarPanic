using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 0f;
    public float maxHealth = 3f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        GameManager.instance.GameOver();
    }
}
