using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public bool gameOver = false, paused = false;

    public HeartUI heartUI;

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
        heartUI = FindFirstObjectByType<HeartUI>();
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

    public void UpdateHealth(float health)
    {
        heartUI.UpdateVisuals(health);
    }
    public void GameOver()
    {
        AudioManager.instance.Stop();
        gameOver = true;
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        Time.timeScale = 0;
        Debug.Log("Player Has Died");
    }

    // TODO @Rick call this from Game Over menu button
    public void Retry()
    {
        gameOver = false;
        if (!ChangeScene.changingScene)
        {
            StartCoroutine(LoadScene("SampleScene"));
        }
    }

    // TODO @Rick call this from Game Over menu button
    public void ToMenu()
    {
        gameOver = false;
        if (!ChangeScene.changingScene)
        {
            StartCoroutine(LoadScene("MainMenu"));
        }
    }

    IEnumerator LoadScene(string scene)
    {
        ScreenWipe.current.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        ChangeScene.changingScene = true;
        ScreenWipe.current.WipeIn();
        yield return new WaitForSecondsRealtime(1f);
        ScreenWipe.current.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
        ChangeScene.changingScene = false;
    }

    // TODO @Rick call this from on screen pause button?
    public void Pause()
    {
        paused = true;
        if (!gameOver)
        {
            AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            Time.timeScale = 0;
            foreach (AudioSource source in sources)
            {
                source.Pause();
            }
        }
    }

    // TODO @Rick call this from pause menu
    public void Unpause()
    {
        paused = false;
        if (!gameOver)
        {
            AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            Time.timeScale = 1;
            foreach (AudioSource source in sources)
            {
                source.UnPause();
            }
        }
        
    }
}
