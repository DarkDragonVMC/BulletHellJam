using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float timeToLive;
    public int damage;
    public bool isBullet;

    public bool globalExplosive;
    public bool explosive;
    public float explosionRadius;

    public int healPercentage;
    public int healAmount;

    float coolDown;

    public GameObject particle;
    public Color customColor;
    public Material customShader;

    void Awake()
    {
        coolDown = Mathf.Infinity;
        Invoke("startExpiring", 0.1f);
    }

    private void startExpiring()
    {
        coolDown = timeToLive;
    }

    private void Update()
    {
        if (SceneManagement.paused) return;
        coolDown -= Time.deltaTime;
        if(coolDown <= 0 && isBullet && !globalExplosive)
        {
            Expire();
        } else if(coolDown <= 0 && isBullet && explosive)
        {
            explode();
        }
    }

    private void Expire()
    {
        if (globalExplosive || explosive) return;
        Destroy(gameObject);
        return;
    }

    private void spawnParticleSystem()
    {
        GameObject p = Instantiate(particle, this.transform.position, Quaternion.identity);
        p.GetComponent<ParticleSystem>().startColor = customColor;
        p.GetComponent<ParticleSystemRenderer>().material = customShader;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag == "AllyBullet")
        {
            if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Enemy2" || collision.gameObject.tag == "Enemy3")
            {
                if (explosive)
                {
                    explode();
                    return;
                }
                else
                {
                    //calculate healing
                    float prob = healPercentage * 0.01f;
                    if (Random.value > (1 - prob)) FindObjectOfType<PlayerHealth>().heal(healAmount);

                    if (collision.gameObject.tag == "Enemy")
                    {
                        collision.GetComponent<EnemyMechanics1>().takeDamage(damage);
                        spawnParticleSystem();
                        Destroy(this.gameObject);
                    }
                    if (collision.gameObject.tag == "Enemy2")
                    {
                        collision.GetComponent<EnemyMechanics2>().takeDamage(damage);
                        spawnParticleSystem();
                        Destroy(this.gameObject);
                    }
                    if (collision.gameObject.tag == "Enemy3")
                    {
                        collision.GetComponent<EnemyMechanics3>().takeDamage(damage);
                        spawnParticleSystem();
                        Destroy(this.gameObject);
                    }
                }
            }
        }

        if (this.gameObject.tag == "EnemyBullet")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
                spawnParticleSystem();
                Destroy(this.gameObject);
            }
        }

        if (this.gameObject.tag == "Enemy3Bullet")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
                spawnParticleSystem();
                Destroy(this.gameObject);
            }
        }

        if (this.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
            }
        }

        if (this.gameObject.tag == "Enemy2")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
            }
        }

        if (this.gameObject.tag == "Enemy3")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
            }
        }
    }

    private void explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);
        explosive = false;
        spawnParticleSystem();
        foreach (Collider2D c in hits) OnTriggerEnter2D(c);
        Destroy(this.gameObject);
    }
}
