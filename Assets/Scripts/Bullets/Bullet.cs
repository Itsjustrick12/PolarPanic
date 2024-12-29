using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MagnetizedObj))]
public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public MagnetizedObj magnet;

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0f;

        if(magnet == null)
        {
            magnet = GetComponent<MagnetizedObj>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
