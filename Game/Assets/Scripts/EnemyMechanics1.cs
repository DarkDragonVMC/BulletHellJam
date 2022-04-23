using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics1 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float posX;
    public float posY;
    public int XorY;
    public float speed;
    public bool StartTriggered = false;
    public bool cycleCompleted = false;

    public static List<Rigidbody2D> BulletSaves = new ();
    public static List<Vector2> BulletVelocitySaves = new ();
    public static List<Rigidbody2D> BulletsToDestroy = new ();
    public static List<Vector2> EnemyVelocitySaves = new ();
    public static List<Rigidbody2D> EnemySaves = new ();
    public static List<Rigidbody2D> EnemiesToDestroy = new();

    bool moving = false;


    public int maxHealth;
    public float timeBetweenShots;
    private float cooldown;
    private int healthPoints;
    private GameObject barrel;
    public GameObject enemyBullet;
    public float bulletForce;

    private PlayerHealth ph;

    public GameObject[] itemDrops;
    public int scoreIncrease;
    public int droppingPercentage;


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
            if(Random.value > (1 - value))
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
        Randomize();
    }


    private void Update()
    {
        if (ph.dead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (SceneManagement.paused)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Move();
        Rotate();
        Shoot();
      
    }

    private void Rotate()
    {
        if (SceneManagement.paused) return;
        Vector2 player = GameObject.Find("Player").transform.position;
        Vector2 lookDir = player - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void Shoot()
    {
        if (ph.dead) return;
        if (SceneManagement.paused) return;
        if (cooldown > 0) cooldown -= Time.deltaTime;
        if (cooldown < 0) cooldown = 0;
        if (cooldown != 0) return;

        GameObject bullet = Instantiate(enemyBullet, barrel.transform.position, barrel.transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(barrel.transform.up * bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play("ShootingEnemy");
        cooldown = timeBetweenShots;
        BulletSaves.Add(bulletRb);
    }

    public static void saveBulletVelocity()
    {
        BulletVelocitySaves.Clear();

        foreach(Rigidbody2D b in BulletSaves)
        {
            if (b == null)
            {
                BulletsToDestroy.Add(b);
            }
            else
            {
                BulletVelocitySaves.Add(b.velocity);
                b.velocity = Vector2.zero;
            }            
        }
        foreach(Rigidbody2D r in BulletsToDestroy)
        {
            BulletSaves.Remove(r);
        }
    }

    public static void loadBulletVelocity()
    {
        foreach(Rigidbody2D b in BulletSaves)
        {
            foreach(Vector2 v in BulletVelocitySaves)
            {
                if(b != null)
                {
                    b.velocity = v;
                }
            }
        }
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

    public static void saveEnemyVelocity()
    {
        EnemyVelocitySaves.Clear();

        foreach (Rigidbody2D e in EnemySaves)
        {
            if (e == null)
            {
                EnemiesToDestroy.Add(e);
            }
            else
            {
                EnemyVelocitySaves.Add(e.velocity);
                e.velocity = Vector2.zero;
            }
        }
        foreach(Rigidbody2D r in EnemiesToDestroy)
        {
            EnemySaves.Remove(r);
        }
    }

    public static void loadEnemyVelocity()
    {
        foreach (Rigidbody2D e in EnemySaves)
        {
            foreach (Vector2 v in EnemyVelocitySaves)
            {
                if(e != null) e.velocity = v;
            }
        }
    }

    public void Move()
    {
        if (!moving)
        {
            Randomize();
        }
        if (ph.dead) return;
        if (SceneManagement.paused)
        {
            return;
        }
        Vector2 MoveX = new Vector2(speed, 0);
        Vector2 MoveY = new Vector2(0, speed);
        if(XorY == 0)
        {

            moving = true;
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
                setToFalse();
                moving = false;
            }

        }
        if (XorY == 1)
        {
            moving = true;
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
                setToFalse();
                moving = false;
            }
        }
    }
    public void Randomize()
    {
        XorY = Random.Range(0, 2);
    }
    
}
