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

    //Health Display
    private Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite emptyHeart2;

    //Game Over Screen
    public float fadeInSpeed;
    private CanvasGroup gameOverScreen;
    private GameObject backToMenuButton;
    private GameObject restartButton;
    private GameObject quitButton;

    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        hearts = new Image[3];
        hearts[0] = GameObject.Find("Heart0").GetComponent<Image>();
        hearts[1] = GameObject.Find("Heart1").GetComponent<Image>();
        hearts[2] = GameObject.Find("Heart2").GetComponent<Image>();

        gameOverScreen = GameObject.Find("GameOver").GetComponent<CanvasGroup>();
        backToMenuButton = GameObject.Find("BackToMenu");
        restartButton = GameObject.Find("Restart");
        quitButton = GameObject.Find("Quit");

        backToMenuButton.SetActive(false);
        restartButton.SetActive(false);
        quitButton.SetActive(false);

        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }

    void Awake()
    {
        healthPoints = startHealth;
    }

    public void takeDamage(int amount)
    {
        if (dead) return;
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
        healthPoints = healthPoints + amount;
        if (healthPoints > maxHealth) healthPoints = maxHealth;
        updateHealthDisplay();
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
        if (dead) return;
        backToMenuButton.SetActive(true);
        restartButton.SetActive(true);
        quitButton.SetActive(true);

        StartCoroutine(fadeIn());

        healthPoints = 0;
        updateHealthDisplay();
        StartCoroutine(cameraShake.Shake(shakeDuration, shakeStrenght));
        FindObjectOfType<AudioManager>().Play("Death");
        dead = true;
    }

    private IEnumerator fadeIn()
    {
        while (gameOverScreen.alpha < 1)
        {
            gameOverScreen.alpha += Time.deltaTime * fadeInSpeed;
            if (gameOverScreen.alpha > 1) gameOverScreen.alpha = 1;
            yield return null;
        }
    }
}
