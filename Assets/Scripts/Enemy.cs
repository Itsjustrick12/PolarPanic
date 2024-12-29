using System.Collections;
using System.Collections.Generic;
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
    //[SerializeField] float fireRate = 1f;
    [Tooltip("Initial speed of bullets when spawned")]
    [SerializeField] float bulletSpeed = 1f;
    [SerializeField] public int bulletPolarity = 1;
    private float shootCooldown = 0f;
    [SerializeField] Transform shotSpawnPoint;

    [SerializeField] AttackPattern attackPattern = null;
    private int attackPatternPos = 0;
    //public List<Coroutine> activeBulletCoroutines = new();

    [SerializeField] SpriteRenderer screenSprite = null;
    [SerializeField] Sprite red;
    [SerializeField] Sprite blue;
    [SerializeField] Sprite white;

    [SerializeField] float maxHealth = 3f;
    private float currHealth = 0f;

    private Vector2 playerDirection;
    private EnemyState state;

    private bool facingRight = true;

    private Transform player;

    private bool isDead = false;

    [SerializeField] GameObject drop = null;
    private void Awake()
    {
        SetPolarity(bulletPolarity);
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
        shootCooldown -= Time.fixedDeltaTime;
        DetermineMovement();
        if (shootCooldown <= 0f && state == EnemyState.Hold)
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
        while(shootCooldown <= 0f)
        {
            shootCooldown = attackPattern.FireAttackPattern(ref attackPatternPos, this, shotSpawnPoint, bulletSpeed, bulletPolarity);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead){

            Debug.Log("Enemy Hit");
            currHealth -= damage;
            GetComponent<SimpleFlash>().Flash(1f, 3, true);

            if (currHealth < 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        isDead = true;
        //Cancel all scheduled bullets bc the enemy died
        StopAllCoroutines();
        /*
        foreach (Coroutine _coroutine in activeBulletCoroutines)
        {
            StopCoroutine(_coroutine);
        }
        */
        EnemySpawner.instance.KillEnemy(1);
        SpawnPickup();
        Destroy(gameObject);
    }

    public void SetPolarity(int polarity)
    {
        bulletPolarity = polarity;

        if (polarity == 0)
        {
            screenSprite.sprite = white;
        }
        else if (polarity > 0)
        {
            //Set Red
            screenSprite.sprite = red;
        }
        else
        {
            screenSprite.sprite = blue;
        }
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
