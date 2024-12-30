using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void UpdateWaves(int amt)
    {
        text.text = amt.ToString();
    }
}
