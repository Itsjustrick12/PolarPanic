using UnityEngine;

public class HeartUI : MonoBehaviour
{
    public GameObject[] hearts;

    public void UpdateVisuals(float health)
    {
        //Debug.Log(health);
        int temp = (int)health;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < temp)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
