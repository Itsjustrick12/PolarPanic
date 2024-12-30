using UnityEngine;

public class UnpauseButton : MonoBehaviour
{
    public void Unpause()
    {
        GameManager.instance.UnpauseButton();
    }
}
