using UnityEngine;
using UnityEngine.UIElements;

public class HeartsUI : MonoBehaviour
{

    public GameObject[] hearts;
    public int numHearts = 10;

    public void Awake()
    {
    }

    public void UpdateVisuals(int health)
    {
        for (int i = 0; i < hearts.Length; i++) {
            
            if ( i < health )
            {

                hearts[i].gameObject.SetActive(true);
            }
            else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }

}
