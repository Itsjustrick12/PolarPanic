using UnityEngine;
using UnityEngine.SceneManagement;

public class ToScene : MonoBehaviour
{
    public int sceneDesired = 0;
    public void LoadScene()
    {
        if (sceneDesired == 0)
        {
            GameManager.instance.ToMenu();
        }
    }
}
