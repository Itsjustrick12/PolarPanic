using UnityEngine;
using TMPro;

public class ScrapUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void UpdateScrap(int amt)
    {
        text.text = amt.ToString();
    }
}
