using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveX, moveY;
    Rigidbody2D rb;
    Vector3 targetVelocity, velocity = Vector3.zero;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = 0.01f;
    [SerializeField] private float speed = 10.0f;
    private float calculatedSpeed = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        calculatedSpeed = speed; // TODO add any speed modifiers here (e.g. dodgeroll)

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
