using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject pivotObj;
    public float radius;
    public int polarity = 1;
    [SerializeField] private Collider2D shieldCollider, magnetCollider;
    [SerializeField] float bounceForce = 1.2f;

    private Transform pivot;

    private MagnetizedObj magnetizedObj;

    void Start()
    {
        magnetizedObj = GetComponent<MagnetizedObj>();
        magnetizedObj.SetPolarity(polarity);
        pivot = pivotObj.transform;
        transform.parent = pivot;
        transform.localPosition = Vector3.zero;
        transform.position += Vector3.up * radius;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 orbVector = Camera.main.WorldToScreenPoint(pivotObj.transform.position);
        orbVector = Input.mousePosition - orbVector;
        float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;

        pivot.position = pivotObj.transform.position;
        pivot.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        bool _positive = Input.GetMouseButton(0);
        bool _negative = Input.GetMouseButton(1);

        int _newPolarity = 0;
        //Neutral polarity
        if ((_positive && _negative) || !(_positive || _negative))
        {
            _newPolarity = 0;
        }
        else if (_positive)
        {
            _newPolarity = 1;
        }
        else
        {
            _newPolarity = -1;
        }

        if(_newPolarity != polarity)
        {
            SetPolarity(_newPolarity);
        }
    }

    private void SetPolarity(int _polarity)
    {
        Color _newColor = Color.magenta;
        switch (_polarity)
        {
            case -1:
                _newColor = Color.blue;
                break;
            case 0:
                _newColor = Color.gray;
                break;
            case 1:
                _newColor = Color.red;
                break;
        }

        polarity = _polarity;
        spriteRenderer.color = _newColor;
        magnetizedObj.SetPolarity(polarity);
    }

    //Hi Evan putting this here for ease of access for modifying the sheilds' polarity obj
    private void TogglePolarity()
    {
        polarity *= -1;
        spriteRenderer.color = polarity == 1 ? Color.red : Color.blue;
        magnetizedObj.SetPolarity(polarity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 9) // bullet layer
        {
            if (other.gameObject.TryGetComponent(out Bullet _hitBullet))
            {
                if(polarity == -_hitBullet.magnet.GetPolarity())
                {
                    //Opposite polarities, stick!
                }
                else
                {
                    //Same or neutral polarities, bounce!
                    _hitBullet.rb.linearVelocity = bounceForce * (new Vector3(_hitBullet.rb.linearVelocity.x, _hitBullet.rb.linearVelocity.y, 0f) - 2f * Vector3.Dot(_hitBullet.rb.linearVelocity, transform.right) * transform.right);
                }
            }

            //Damage(1); // technically we could make some bullets do greater damage here, hmm
        }
    }

    public void SetActive(bool active)
    {
        spriteRenderer.enabled = active;
        magnetCollider.enabled = active;
        shieldCollider.enabled = active;
    }
}