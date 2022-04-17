using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    private int healthPoints;
    public int maxHealth;
    public int startHealth;

    //Health Display
    private Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite emptyHeart2;

    // Start is called before the first frame update
    void Start()
    {
        hearts[0] = GameObject.Find("Heart0").GetComponent<Image>();
        hearts[1] = GameObject.Find("Heart1").GetComponent<Image>();
        hearts[2] = GameObject.Find("Heart2").GetComponent<Image>();
    }

    void Awake()
    {
        healthPoints = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        healthPoints = healthPoints - amount;
        updateHealthDisplay();
        if (healthPoints <= 0)
        {
            Die();
            return;
        }
    }

    public void heal(int amount)
    {
        healthPoints = healthPoints + amount;
        if (healthPoints > 3) healthPoints = 3;
        updateHealthDisplay();
    }

    public void updateHealthDisplay()
    {
        int easterEgg = Random.Range(1, 90);

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

            hearts[1].sprite = fullHeart;
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

    }
}
