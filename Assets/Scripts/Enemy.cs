using UnityEngine;

public enum EnemyState
{
    RunAway,
    Hold,
    MoveTowards
}


public class Enemy : MonoBehaviour
{
    
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float runAwayMult = 1.25f;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] float minDistance = 3f;
    [SerializeField] float shootRange = 5f;
    [SerializeField] float fireRate = 1f;

    [SerializeField] float maxHealth = 3f;
    private float currHealth = 0f;

    private Vector2 playerDirection;
    private EnemyState state;

    private Transform player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.instance.player.transform;
        currHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 toPlayer = player.position - transform.position;
        playerDirection = toPlayer.normalized;
        
        if (toPlayer.magnitude <= minDistance)
        {
            //Run Away
            state = EnemyState.RunAway;
            
        }
        else if (toPlayer.magnitude <= shootRange)
        {
            //stay put and shoot at player
            state = EnemyState.Hold;
        }
        else
        {
            //move towards player
            state = EnemyState.MoveTowards;
        }
    }

    private void FixedUpdate()
    {
        DetermineMovement();
    }

    private void DetermineMovement()
    {
        switch (state)
        {
            case EnemyState.RunAway:
                rb.linearVelocity = playerDirection * runAwayMult * -moveSpeed;
                break;
            case EnemyState.Hold:
                rb.linearVelocity = Vector2.zero;
                break;
            case EnemyState.MoveTowards:
                rb.linearVelocity = playerDirection * moveSpeed;
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;

        if (currHealth < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject);
    }
}
