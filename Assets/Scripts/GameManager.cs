using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public bool gameOver = false, paused = false;

    public int nuts = 0;
    public SoundClip pickupSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ChangeScene.changingScene || !ScreenWipe.over)
            {
                return;
            }

            if (MainMenuManager.inMainMenu)
            {
                return;
            }

            if (!paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }       
    }

    public void HealPlayer(int amt)
    {
        player.GetComponentInChildren<PlayerHealth>().Heal(amt);
    }


    public void UpdateNuts(int amt)
    {
        nuts += amt;
        player.GetComponentInChildren<SoundPlayer>().PlaySound(pickupSound);
    }

    public void GameOver()
    {
        AudioManager.instance.Stop();
        gameOver = true;
        Debug.Log("Player Has Died");
    }

    public void Retry()
    {
        gameOver = false;
        if (!ChangeScene.changingScene)
        {
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        ChangeScene.changingScene = true;
        ScreenWipe.current.WipeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");
        ChangeScene.changingScene = false;
    }

    public void Pause()
    {
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        paused = true;
        Time.timeScale = 0;
        foreach (AudioSource source in sources)
        {
            source.Pause();
        }
    }

    public void Unpause()
    {
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        paused = false;
        Time.timeScale = 1;
        foreach (AudioSource source in sources)
        {
            source.UnPause();
        }
    }
}
