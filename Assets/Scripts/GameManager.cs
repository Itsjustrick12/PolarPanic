using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public bool gameOver = false, paused = false;

    [SerializeField] GameObject PauseScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject darkScreen;

    public HeartUI heartUI;
    public ScrapUI scrapUI;
    public WaveUI waveUI;

    public int nuts = 0;
    public SoundClip pickupSound, buttonClick;
    public SoundPlayer soundPlayer;

    public bool gameScene = true;

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

        if (gameScene)
        {
            heartUI = FindFirstObjectByType<HeartUI>();
            scrapUI = FindFirstObjectByType<ScrapUI>();
            waveUI = FindFirstObjectByType<WaveUI>();
            darkScreen.SetActive(false);
            PauseScreen.SetActive(false);
            gameOverScreen.SetActive(false);
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

            if (gameScene)
            {

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
    }

    public void HealPlayer(int amt)
    {
        player.GetComponentInChildren<PlayerHealth>().Heal(amt);
    }


    public void UpdateNuts(int amt)
    {
        nuts += amt;
        scrapUI.UpdateScrap(nuts);
        player.GetComponentInChildren<SoundPlayer>().PlaySound(pickupSound);
    }

    public void UpdateWaves(int amt)
    {
        waveUI.UpdateWaves(amt);
    }

    public void UpdateHealth(float health)
    {
        heartUI.UpdateVisuals(health);
    }
    public void GameOver()
    {
        AudioManager.instance.Stop();
        gameOverScreen.SetActive(true);
        darkScreen.SetActive(true);
        gameOver = true;
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        Time.timeScale = 0;
        Debug.Log("Player Has Died");
    }

    // TODO @Rick call this from Game Over menu button
    public void Retry()
    {
        soundPlayer.PlaySound(buttonClick);
        gameOver = false;
        MainMenuManager.inMainMenu = false;
        if (!ChangeScene.changingScene)
        {
            StartCoroutine(LoadScene("GameScene"));
        }
    }

    // TODO @Rick call this from Game Over menu button
    public void ToMenu()
    {
        soundPlayer.PlaySound(buttonClick);
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
        AudioManager.instance.FadeOutCurrent();
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
            PauseScreen.SetActive(true);
            darkScreen.SetActive(true);

        }
    }

    public void UnpauseButton()
    {
        soundPlayer.PlaySound(buttonClick);
        Unpause();
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
            PauseScreen.SetActive(false);
            darkScreen.SetActive(false);
        }
        
    }
}
