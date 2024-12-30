using UnityEngine;

enum Direction
{
    LeftRight,
    Up,
    Down
}
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
    [SerializeField] Animator anim;
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private SoundClip roll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        dodgerollTime = float.PositiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused) return;

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        if (!dodgeroll)
        {
            if (moveY > 0){
                ResetAnimBools();
                anim.SetBool("Up", true);
            }
            else if (moveY < 0)
            {
                ResetAnimBools();
                anim.SetBool("Down", true);
            
            }
            else if (moveX == 0 && moveY == 0)
            {
                ResetAnimBools();
            }
            else if (moveX > 0)
            {
                ResetAnimBools();
                anim.SetBool("Right", true);
            
            }
            else
            {
                ResetAnimBools();
                anim.SetBool("Left", true);
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && !dodgeroll && !(moveX == 0 && moveY == 0))
        {
            dodgeroll = true;
            soundPlayer.PlaySound(roll);

            ResetAnimBools();
            anim.SetBool("Roll",true);


            if (moveX > 0)
            {
                anim.SetBool("Right", true);
            }
            else if (moveX < 0)
            {
                anim.SetBool("Left", true);
            }
            else if (moveY > 0)
            {
                anim.SetBool("Up", true);
            }
            else if (moveY < 0)
            {
                anim.SetBool("Down", true);
            }
            canDodgeroll = false;
            dodgerollTime = Time.time + dodgerollLength;
            dodgerollResetTime = Time.time + dodgerollLength + dodgerollCooldown;
            Physics2D.IgnoreLayerCollision(7, 9, true);
            //shield.SetActive(false);
        }

        if (dodgeroll && Time.time >= dodgerollTime)
        {
            dodgeroll = false;
            //shield.SetActive(true);
            Physics2D.IgnoreLayerCollision(7, 9, false);
            Physics2D.IgnoreLayerCollision(7, 8, false);
        }

        if (!canDodgeroll && Time.time >= dodgerollResetTime)
        {
            canDodgeroll = true;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.paused) return;
        
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

    public void ResetAnimBools()
    {
        anim.SetBool("Up", false);
        anim.SetBool("Down", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);
        anim.SetBool("Roll", false);
    }
}
