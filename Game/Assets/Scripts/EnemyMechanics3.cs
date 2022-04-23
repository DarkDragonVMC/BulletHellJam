using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics3 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float posX;
    public float posY;
    public int XorY;
    public float speed;
    public bool StartTriggered = false;
    public bool cycleCompleted = false;

    public int maxHealth;
    public float timeBetweenShots;
    private float cooldown;
    private int healthPoints;
    private GameObject barrel;
    public GameObject enemyBullet;
    public float bulletForce;
    public float rotationSpeed;

    public GameObject[] itemDrops;
    public int scoreIncrease;
    public int droppingPercentage;

    private PlayerHealth ph;


    private void Awake()
    {
        healthPoints = maxHealth;
        posX = this.transform.position.x;
        posY = this.transform.position.y;
        barrel = this.transform.GetChild(0).gameObject;
        cooldown = timeBetweenShots;
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    public void takeDamage(int amount)
    {
        healthPoints = healthPoints - amount;
        FindObjectOfType<AudioManager>().Play("TakeDamageEnemy");
        if (healthPoints <= 0)
        {
            float value = droppingPercentage * 0.01f;
            if (Random.value > (1 - value))
            {
                int itemNumber = Random.Range(0, itemDrops.Length);
                Instantiate(itemDrops[itemNumber], this.gameObject.transform.position, this.gameObject.transform.rotation);
            }
            Destroy(this.gameObject);
            Score.UpdateScore(scoreIncrease);

            return;
        }
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        if (ph.dead) return;
        if (SceneManagement.paused) return;
        rb.rotation = rb.rotation + rotationSpeed * Time.deltaTime;
        Shoot();
    }

    

    private void Shoot()
    {
        if (ph.dead) return;
        if (SceneManagement.paused)
        {
            return;
        }
        if (cooldown > 0) cooldown -= Time.deltaTime;
        if (cooldown < 0) cooldown = 0;
        if (cooldown != 0) return;

        GameObject bullet = Instantiate(enemyBullet, barrel.transform.position, barrel.transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(barrel.transform.up * bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("ShootingEnemy");
        cooldown = timeBetweenShots;
        EnemyMechanics1.BulletSaves.Add(bullet.GetComponent<Rigidbody2D>());
    }

    public void setToFalse()
    {
        StartTriggered = false;
        cycleCompleted = false;
    }
    public void triggerStart()
    {
        StartTriggered = true;
    }
    public void completeCycle()
    {
        cycleCompleted = true;
    }
    public void teleportBack()
    {
        rb.transform.position = new Vector3(posX, posY, 0);
    }

    public void Move()
    {
        if (ph.dead) return;
        if (SceneManagement.paused) return;
        Vector2 MoveX = new Vector2(speed, 0);
        Vector2 MoveY = new Vector2(0, speed);
        if(XorY == 0)
        {

            if (this.transform.position.x == posX && StartTriggered == false)
            {
                rb.velocity = MoveX;
                Invoke("triggerStart", 0.5f);
                                
            }
            if(this.transform.position.x >= posX + 5)
            {
                rb.velocity = -MoveX;
            }
            if (this.transform.position.x <= posX - 5)
            {
                rb.velocity = MoveX;
                
            }
            if (this.transform.position.x >= posX - 0.1f && this.transform.position.x <= posX + 0.1f && StartTriggered && !cycleCompleted)
            {
                Invoke("completeCycle", 0.5f);
            }
            if (this.transform.position.x <= posX + 0.1f && this.transform.position.x >= posX - 0.1f && cycleCompleted && StartTriggered)
            {
                rb.velocity = new Vector2(0, 0);
                teleportBack();
                Randomize();
                setToFalse();
            }

        }
        if (XorY == 1)
        {

            if (this.transform.position.y == posY && StartTriggered == false)
            {
                rb.velocity = MoveY;
                Invoke("triggerStart", 0.5f);

            }
            if (this.transform.position.y >= posY + 5)
            {
                rb.velocity = -MoveY;
            }
            if (this.transform.position.y<= posY - 5)
            {
                rb.velocity = MoveY;

            }
            if (this.transform.position.y >= posY - 0.1f && this.transform.position.y <= posY + 0.1f && StartTriggered && !cycleCompleted)
            {
                Invoke("completeCycle", 0.5f);
            }
            if (this.transform.position.y <= posY + 0.1f && this.transform.position.y >= posY - 0.1f && cycleCompleted && StartTriggered)
            {
                rb.velocity = new Vector2(0, 0);
                teleportBack();
                Randomize();
                setToFalse();
            }
        }
    }
    public void Randomize()
    {
        XorY = Random.Range(0, 2);
    }
    
}
