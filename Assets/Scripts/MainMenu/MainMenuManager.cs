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
    public MusicClip menuMusic, menuMusic2;
    public GameObject CreditsPanel;
    public SoundPlayer soundPlayer;
    public SoundClip buttonClick, buttonBack;

    public void Awake()
    {
        // ScreenWipe.current.PostUnwipe += PlayMenuMusic;
    }

    // public void PlayMenuMusic()
    // {
    //     int rand = Random.Range(0, 2);
    //     if (rand == 1)
    //         AudioManager.instance.ChangeBGM(menuMusic, 0);
    //     else
    //         AudioManager.instance.ChangeBGM(menuMusic2, 0);
    // }

    public void OpenCredits()
    {
        soundPlayer.PlaySound(buttonClick);
        CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        soundPlayer.PlaySound(buttonBack);
        CreditsPanel.SetActive(false);
    }
}