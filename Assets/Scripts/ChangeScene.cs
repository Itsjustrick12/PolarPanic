using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ChangeScene : MonoBehaviour
{
    public string scene;
    public string spawn;
    public static bool changingScene = false;
    public static Action changeScene;

    IEnumerator LoadNextScene()
    {
        changingScene = true;
        ScreenWipe.current.WipeIn();
        AudioManager.instance.FadeOutCurrent();
        yield return new WaitForSeconds(1f);
        DisableMenuMusic();
        SceneManager.LoadScene(scene);
    }

    public static void ChangeSceneMinimal(string scene)
    {
        changingScene = true;
        DisableMenuMusic();
        SceneManager.LoadScene(scene);
        changingScene = false;
    }

    static IEnumerator LoadSceneEnum(string scene)
    {
        changingScene = true;
        ScreenWipe.current.WipeIn();
        yield return new WaitForSeconds(1f);
        DisableMenuMusic();
        SceneManager.LoadScene(scene);
    }

    public static void DisableMenuMusic()
    {
        if (MainMenuManager.inMainMenu)
        {
            AudioManager.instance.Stop();
            MainMenuManager.inMainMenu = false;
        }
    }
}