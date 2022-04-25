using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Rendering.Universal;

public class SceneManagement : MonoBehaviour
{
    public static bool paused;
    public CanvasGroup pauseMenu;
    public float fadeInSpeed;

    public GameObject backToMenuButton;
    public GameObject restartButton;
    public GameObject quitButton;
    public CanvasGroup pausedScreen;

    bool fading = false;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SettingsManager.loadSettings(GameObject.Find("Global Volume (Effects)").GetComponent<Volume>());
            Score.updateHighScoreDisplay(SceneManager.GetSceneByBuildIndex(0), SceneManager.GetActiveScene());
        }
        if (pausedScreen) pausedScreen.gameObject.SetActive(false);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += Score.updateHighScoreDisplay;
        if (SceneManager.GetActiveScene().buildIndex != 1) return;

        backToMenuButton.SetActive(false);
        quitButton.SetActive(false);
        restartButton.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                StartCoroutine(fadeOut());
            }
            else
            {
                if (FindObjectOfType<PlayerHealth>().dead) return;
                StartCoroutine(fadeIn());
            }
        }
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void openTutorial()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void startGame()
    {
        SceneManager.LoadScene("Prototype");
        paused = false;
        fading = false;
        Time.timeScale = 1;
    }
    public void quit()
    {
        Application.Quit();
    }

    private IEnumerator fadeIn()
    {
        if (!fading)
        {
            fading = true;
            Score.scoreGo.SetActive(false);
            pausedScreen.gameObject.SetActive(true);
            backToMenuButton.SetActive(true);
            quitButton.SetActive(true);
            restartButton.SetActive(true);

            //EnemyMechanics1.saveBulletVelocity();
            //EnemyMechanics1.saveEnemyVelocity();

            paused = true;


            while (pauseMenu.alpha < 1)
            {
                pauseMenu.alpha += Time.deltaTime * fadeInSpeed;
                if (pauseMenu.alpha > 1) pauseMenu.alpha = 1;
                yield return null;
            }
            Time.timeScale = 0;
            fading = false;
        }
    }

    public IEnumerator fadeOut()
    {
        if (!fading)
        {
            fading = true;
            Score.scoreGo.SetActive(true);
            backToMenuButton.SetActive(false);
            quitButton.SetActive(false);
            restartButton.SetActive(false);

            //EnemyMechanics1.loadBulletVelocity();
            //EnemyMechanics1.loadEnemyVelocity();

            paused = false;
            Time.timeScale = 1;

            while (pauseMenu.alpha > 0)
            {
                pauseMenu.alpha -= Time.deltaTime * fadeInSpeed;
                if (pauseMenu.alpha < 0) pauseMenu.alpha = 0;
                yield return null;
            }
            pausedScreen.gameObject.SetActive(false);
            fading = false;
        }
    }
}
