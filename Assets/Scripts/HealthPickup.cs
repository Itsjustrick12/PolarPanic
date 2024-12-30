using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int value = 1;
    [SerializeField] MagnetizedObj magnet;
    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] SoundClip dropHeal;

    private void Start()
    {
        soundPlayer.PlaySound(dropHeal);
        Debug.Log("Played sound");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Had to do a workaround and make a point with a seperate collider than the magenetism script on the play
        if (collision.gameObject.CompareTag("PickupPoint"))
        {
            GameManager.instance.HealPlayer(value);
            DestroyPickup();
        }
    }

    public void DestroyPickup()
    {
        magnet.OnDestroy?.Invoke(magnet);
        Destroy(gameObject);
    }
}
