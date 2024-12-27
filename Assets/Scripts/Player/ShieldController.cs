using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject pivotObj;
    public float radius;
    public int polarity = 1;

    private Transform pivot;

    void Start()
    {
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
            polarity *= -1;
            spriteRenderer.color = polarity == 1 ? Color.red : Color.blue;
        }
    }
}