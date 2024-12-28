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
    private float shootCooldown = 0f;

    [SerializeField] float maxHealth = 3f;
    private float currHealth = 0f;

    private Vector2 playerDirection;
    private EnemyState state;

    private bool facingRight = true;

    private Transform player;

    [SerializeField] GameObject drop = null;
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

        float toPlayerX = toPlayer.x;

        //Flip Sprite of enemy towards the movement vector
        if ((state == EnemyState.MoveTowards || state == EnemyState.RunAway)
            && (toPlayerX > 0 && !facingRight || (toPlayerX < 0 && facingRight)))
        {
            facingRight = !facingRight;
            Vector3 newScale = new Vector3 (transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
    }

    private void FixedUpdate()
    {
        shootCooldown += Time.deltaTime;
        DetermineMovement();
        if (shootCooldown >= fireRate && state == EnemyState.Hold)
        {
            ShootTowardsPlayer();
        }
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

    //PLACE BULLET SPAWN HERE ONCE PREFAB IS MADE
    public void ShootTowardsPlayer()
    {
        //Debug.Log("Enemy Shoots a Bullet");
        shootCooldown = 0f;
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
        SpawnPickup();
        Destroy(gameObject);
    }

    private void SpawnPickup()
    {
        GameObject newPickup = Instantiate(drop, transform);
        GameObject temp = GameObject.Find("PickupsContainer");

        if (temp != null)
        {

            newPickup.transform.parent = temp.transform;
        }
    }
}
