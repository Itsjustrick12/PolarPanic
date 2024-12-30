using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    public float blinkTime;
    private float timer = 0;
    public GameObject obj;
    public Vector2 offset;
    private SpriteRenderer sprite;
    public float minCooldown = 0.1f;
    public float maxCooldown = 0.4f;

    public SpawnCircle(float blinkTime, GameObject obj)
    {
        this.blinkTime = blinkTime;
        this.obj = obj;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.paused || GameManager.instance.gameOver) return;

        timer += Time.deltaTime;

        if (timer > blinkTime)
        {
            SpawnAndDestroy();
        }
    }

    public void SpawnAndDestroy()
    {
        if(Instantiate(obj, transform.position, Quaternion.identity).TryGetComponent(out Enemy e))
        {
            e.shootCooldown = Random.Range(minCooldown, maxCooldown);
        }

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
