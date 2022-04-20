using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics2 : MonoBehaviour    
{
    int UpDownLeftRight;
    int timerSpeed = 1;
    float timer;
    public Rigidbody2D rb;
    public float speed;

    public int maxHealth;
    public float timeBetweenShots;
    private float cooldown;
    private int healthPoints;
    private GameObject barrel;
    public GameObject enemyBullet;
    public float bulletForce;

    private PlayerHealth ph;

    public GameObject[] itemDrops;
    public int droppingPercentage;

    private void Awake()
    {
        healthPoints = maxHealth;
        barrel = this.transform.GetChild(0).gameObject;
        cooldown = timeBetweenShots;
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }


    public void Update()
    {
        timer = timer + timerSpeed * Time.deltaTime;
        if(timer > 2)
        {
            timer = 0;
        }
        Move();
        Rotate();
        Shoot();
    }
    public void Move()
    {
        Vector2 upDown = new Vector2(0, speed);
        Vector2 leftRight = new Vector2(speed, 0);
        if (timer == 0)
        {
            Randomize();
        }
        if(UpDownLeftRight == 0)
        {
            rb.velocity = -upDown;
        }
        if (UpDownLeftRight == 1)
        {
            rb.velocity = -leftRight;
        }
        if (UpDownLeftRight == 2)
        {
            rb.velocity = upDown;
        }
        if (UpDownLeftRight == 3)
        {
            rb.velocity = leftRight;
        }
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
            return;
        }
    }
    private void Shoot()
    {
        if (cooldown > 0) cooldown -= Time.deltaTime;
        if (cooldown < 0) cooldown = 0;
        if (cooldown != 0) return;

        GameObject bullet = Instantiate(enemyBullet, barrel.transform.position, barrel.transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(barrel.transform.up * bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("ShootingEnemy");
        cooldown = timeBetweenShots;
    }

    private void Rotate()
    {
        Vector2 player = GameObject.Find("Player").transform.position;
        Vector2 lookDir = player - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    public void Randomize()
    {
        UpDownLeftRight = Random.Range(0, 4);
    }

}
