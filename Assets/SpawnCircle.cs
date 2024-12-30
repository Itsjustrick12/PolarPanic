using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    public float blinkTime;
    private float timer = 0;
    public GameObject obj;
    public Vector2 offset;
    private SpriteRenderer sprite;

    public SpawnCircle(float blinkTime, GameObject obj)
    {
        this.blinkTime = blinkTime;
        this.obj = obj;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer > blinkTime)
        {
            SpawnAndDestroy();
        }
    }

    public void SpawnAndDestroy()
    {
        GameObject.Instantiate(obj, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void SetObj(GameObject newObj)
    {
        sprite = GetComponent<SpriteRenderer>();

        obj = newObj;

        Enemy enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            float polarity = enemy.bulletPolarity;

            if (polarity == 0)
            {
                sprite.color = Color.white;
            }
            else if (polarity > 0)
            {
                sprite.color = new Color(229 / 255f, 45 / 255f, 64 / 255f);
            }
            else
            {
                sprite.color = new Color(25 / 255f, 186 / 255f, 255 / 255f);
            }
        }
    }
}
