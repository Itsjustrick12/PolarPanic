using UnityEngine;
using UnityEngine.UI;

public class ChargeBarFlash : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] private float duration;
    private float t = 0f;
    private Color color;

    public void AddCharge()
    {
        color = new Color(1f, 216f / 255f, 119f / 255f, 1f);
        //duration = _d;
        t = duration;
    }

    public void RemoveCharge()
    {
        color = new Color(185f / 255f, 124f / 255f, 1f, 1f);
        //duration = _d;
        t = duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused) return;
        
        t -= Time.deltaTime;
        if (t < 0f)
        {
            t = 0f;
        }

        color.a = t / duration;
        rawImage.color = color;
    }
}
