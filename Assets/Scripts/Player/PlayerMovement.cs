using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveX, moveY;
    Rigidbody2D rb;
    BoxCollider2D col;

    Vector3 targetVelocity, velocity = Vector3.zero;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.01f;
    [SerializeField] private float speed = 10.0f;
    private float calculatedSpeed = 10.0f;
    public static bool dodgeroll = false;
    private bool canDodgeroll = true;
    private float dodgerollTime, dodgerollResetTime;
    [SerializeField] private float dodgerollLength = 0.3f;
    [SerializeField] private float dodgerollCooldown = 0.3f;
    [SerializeField] private float dodgerollSpeedMultiplier = 1.3f;
    [SerializeField] private ShieldController shield;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        dodgerollTime = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dodgeroll && !(moveX == 0 && moveY == 0))
        {
            dodgeroll = true;
            canDodgeroll = false;
            dodgerollTime = Time.time + dodgerollLength;
            dodgerollResetTime = Time.time + dodgerollLength + dodgerollCooldown;
            Physics2D.IgnoreLayerCollision(7, 9, true);
            shield.SetActive(false);
        }

        if (dodgeroll && Time.time >= dodgerollTime)
        {
            dodgeroll = false;
            shield.SetActive(true);
            Physics2D.IgnoreLayerCollision(7, 9, false);
        }

        if (!canDodgeroll && Time.time >= dodgerollResetTime)
        {
            canDodgeroll = true;
        }
    }

    private void FixedUpdate()
    {
        calculatedSpeed = speed;
        if (dodgeroll)
        {
            calculatedSpeed *= dodgerollSpeedMultiplier;
        }

        targetVelocity = new Vector2(moveX * calculatedSpeed, moveY * calculatedSpeed);
        if (targetVelocity.magnitude > calculatedSpeed)
        {
            targetVelocity = targetVelocity.normalized * calculatedSpeed;
        }
        if (movementSmoothing > 0)
        {
            rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, movementSmoothing * 2.5f);
        }
        else
        {
            rb.linearVelocity = targetVelocity;
        }
    }
}
