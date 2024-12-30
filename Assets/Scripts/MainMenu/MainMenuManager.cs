using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using System.IO;

//holds functions of the main menu and sub menus
public class MainMenuManager : MonoBehaviour
{
    public static bool inMainMenu = false;
    public MusicClip menuMusic;

    public void Start()
    {
        ScreenWipe.current.PostWipe += PlayMenuMusic;
    }

   public void PlayMenuMusic()
   {
        AudioManager.instance.ChangeBGM(menuMusic);
   }
}