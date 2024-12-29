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

    private Transform pivot;

    private MagnetizedObj magnetizedObj;

    void Start()
    {
        magnetizedObj = GetComponent<MagnetizedObj>();
        magnetizedObj.SetPolarity(polarity);
        pivot = pivotObj.transform;
        transform.parent = pivot;
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

        if (Input.GetMouseButtonDown(0))
        {
            TogglePolarity();
        }
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
            if (other.gameObject.GetComponent<MagnetizedObj>().GetPolarity() == polarity)
            {
                Debug.Log("TODO stick bullet of same polarity to shield");
            }
        }
    }

    public void SetActive(bool active)
    {
        spriteRenderer.enabled = active;
        magnetCollider.enabled = active;
        shieldCollider.enabled = active;
    }
}