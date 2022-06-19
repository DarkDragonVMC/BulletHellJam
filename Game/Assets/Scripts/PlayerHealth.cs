using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int healthPoints;
    public int maxHealth;
    public int startHealth;

    private CameraShake cameraShake;

    public float shakeDuration;
    public float shakeStrenght;

    private SceneManagement sm;

    //Health Display
    private Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite emptyHeart2;

    //Game Over Screen
    public float fadeInSpeed;
    public CanvasGroup gameOverScreen;
    public GameObject backToMenuButton;
    public GameObject restartButton;
    public GameObject quitButton;
    public GameObject pauseMenu;

    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new Image[3];
        hearts[0] = GameObject.Find("Heart0").GetComponent<Image>();
        hearts[1] = GameObject.Find("Heart1").GetComponent<Image>();
        hearts[2] = GameObject.Find("Heart2").GetComponent<Image>();

        sm = GameObject.Find("SceneManager").GetComponent<SceneManagement>();

        backToMenuButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);

        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }

    void Awake()
    {
        healthPoints = startHealth;
    }

    public void takeDamage(int amount)
    {
        if (dead) return;
        if (Time.timeScale == 0) return;
        healthPoints = healthPoints - amount;
        if (healthPoints < 0) healthPoints = 0;
        updateHealthDisplay();
        FindObjectOfType<AudioManager>().Play("TakeDamage");
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeStrenght));
        if (healthPoints <= 0)
        {
            Die();
            return;
        }
    }

    public void heal(int amount)
    {
        if (dead) return;
        if (healthPoints >= maxHealth) return;
        healthPoints = healthPoints + amount;
        if (healthPoints > maxHealth) healthPoints = maxHealth;
        updateHealthDisplay();
        FindObjectOfType<AudioManager>().Play("ItemPickup");
    }

    public void updateHealthDisplay()
    {
        int easterEgg = Random.Range(1, 71);

        if(healthPoints == 3)
        {
            foreach(Image h in hearts)
            {
                h.sprite = fullHeart;
            }
        }

        if(healthPoints == 2)
        {
            if (easterEgg == 1) hearts[0].sprite = emptyHeart2;
            else hearts[0].sprite = emptyHeart;

            hearts[1].sprite = fullHeart;
            hearts[2].sprite = fullHeart;
        }

        if(healthPoints == 1)
        {
            if (easterEgg == 1)
            {
                hearts[0].sprite = emptyHeart2;
                hearts[1].sprite = emptyHeart2;
            } else
            {
                hearts[0].sprite = emptyHeart;
                hearts[1].sprite = emptyHeart;
            }

            hearts[2].sprite = fullHeart;
        }

        if (healthPoints == 0)
        {
            if (easterEgg == 1)
            {
                hearts[0].sprite = emptyHeart2;
                hearts[1].sprite = emptyHeart2;
                hearts[2].sprite = emptyHeart2;
            }
            else
            {
                hearts[0].sprite = emptyHeart;
                hearts[1].sprite = emptyHeart;
                hearts[2].sprite = emptyHeart;
            }
        }
    }

    public void Die()
    {
        Score.scoreGo.SetActive(false);
        if (dead) return;
        pauseMenu.SetActive(false);
        backToMenuButton.SetActive(true);
        restartButton.SetActive(true);
        quitButton.SetActive(true);
        Score.scoreGo.SetActive(false);

        StartCoroutine(sm.fadeOut());

        StartCoroutine(fadeIn());

        healthPoints = 0;
        updateHealthDisplay();
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeStrenght));
        Invoke("PlayDeathSound", 0.4f);
        dead = true;
    }

    private IEnumerator fadeIn()
    {
        gameOverScreen.gameObject.SetActive(true);
        Score.scoreGo.SetActive(false);
        Time.timeScale = 0;
        while (gameOverScreen.alpha < 1)
        {
            gameOverScreen.alpha += Time.unscaledDeltaTime * fadeInSpeed;
            if (gameOverScreen.alpha > 1) gameOverScreen.alpha = 1;
            yield return null;
        }
    }
    public void PlayDeathSound()
    {
        FindObjectOfType<AudioManager>().Play("Death");
    }
}
