using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject pivotObj;
    public float radius;
    public int polarity = 1;
    public int health = 5;
    public int maxHealth = 5;
    public bool broken = false;
    [SerializeField] private Collider2D shieldCollider, magnetCollider;

    private Transform pivot;

    void Start()
    {
        pivot = pivotObj.transform;
        transform.parent = pivot;
        transform.position += Vector3.up * radius;
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        broken = false;
    }

    void Update()
    {
        Vector3 orbVector = Camera.main.WorldToScreenPoint(pivotObj.transform.position);
        orbVector = Input.mousePosition - orbVector;
        float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;

        pivot.position = pivotObj.transform.position;
        pivot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (Input.GetMouseButtonDown(0))
        {
            polarity *= -1;
            spriteRenderer.color = polarity == 1 ? Color.red : Color.blue;
        }

        // TODO debug keybinds
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(1);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Damage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && !broken) // bullet layer
        {
            Debug.Log("TODO repel bullet if polarities are equal");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9 && !broken) // bullet layer
        {
            Damage(1); // technically we could make some bullets do greater damage here, hmm
        }
    }

    private void Damage(int amount)
    {
        if (broken) return;
        health = Math.Max(health - amount, 0);
        if (health <= 0)
        {
            Break();
        }
    }

    private void Heal(int amount)
    {
        health = Math.Min(health + amount, maxHealth);
        if (health > 0 && broken)
        {
            Restore();
        }
    }

    private void Break()
    {
        broken = true;
        spriteRenderer.enabled = false;
        magnetCollider.enabled = false;
        shieldCollider.enabled = false;
        // TODO particle system, sounds, etc.
    }

    private void Restore()
    {
        broken = false;
        spriteRenderer.enabled = true;
        magnetCollider.enabled = true;
        shieldCollider.enabled = true;
    }
}