using UnityEditor;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public SoundPlayer soundPlayer;
    public SoundClip buttonClick;

    public void Quit()
    {
        soundPlayer.PlaySound(buttonClick);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
