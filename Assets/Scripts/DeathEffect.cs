using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public SoundClip deathSound;
    public SoundPlayer soundPlayer;
    public float duration = 2f;
    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        if (soundPlayer != null && deathSound != null)
            soundPlayer.PlaySound(deathSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused) return;
        
        if (Time.time >= startTime + duration)
        {
            Destroy(gameObject);
        }
    }
}
