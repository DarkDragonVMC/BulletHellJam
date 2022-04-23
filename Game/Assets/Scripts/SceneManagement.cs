using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public static bool paused;
    public CanvasGroup pauseMenu;
    public float fadeInSpeed;

    public GameObject backToMenuButton;
    public GameObject restartButton;
    public GameObject quitButton;

    bool fading = false;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") SettingsManager.loadSettings(GameObject.Find("Global Volume (Effects)").GetComponent<Volume>());
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;

        backToMenuButton.SetActive(false);
        quitButton.SetActive(false);
        restartButton.SetActive(false);
    }

    private void Update()
    {
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
        Debug.Log("Wohoo");
    }
    public void openTutorial()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void startGame()
    {
        SceneManager.LoadScene("Prototype");
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
            backToMenuButton.SetActive(true);
            quitButton.SetActive(true);
            restartButton.SetActive(true);

            EnemyMechanics1.saveBulletVelocity();
            EnemyMechanics1.saveEnemyVelocity();

            paused = true;


            while (pauseMenu.alpha < 1)
            {
                pauseMenu.alpha += Time.deltaTime * fadeInSpeed;
                if (pauseMenu.alpha > 1) pauseMenu.alpha = 1;
                yield return null;
            }
            fading = false;
        }
    }

    public IEnumerator fadeOut()
    {
        if (!fading)
        {
            fading = true;
            backToMenuButton.SetActive(false);
            quitButton.SetActive(false);
            restartButton.SetActive(false);

            EnemyMechanics1.loadBulletVelocity();
            EnemyMechanics1.loadEnemyVelocity();

            paused = false;

            while (pauseMenu.alpha > 0)
            {
                pauseMenu.alpha -= Time.deltaTime * fadeInSpeed;
                if (pauseMenu.alpha < 0) pauseMenu.alpha = 0;
                yield return null;
            }
            fading = false;
        }
    }
}
