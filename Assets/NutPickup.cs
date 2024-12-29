using UnityEngine;

public class NutPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Had to do a workaround and make a point with a seperate collider than the magenetism script on the play
        if (collision.gameObject.CompareTag("PickupPoint"))
        {
            GameManager.instance.UpdateNuts(value);
            Destroy(gameObject);
        }
    }
}
