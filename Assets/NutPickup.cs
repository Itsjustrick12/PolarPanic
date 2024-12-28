using UnityEngine;

public class NutPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.UpdateNuts(value);
            Destroy(gameObject);
        }
    }
}
