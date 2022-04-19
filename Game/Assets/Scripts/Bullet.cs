using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float timeToLive;
    public int damage;
    public bool isBullet;

    public GameObject particle;
    public Color customColor;
    public Material customShader;

    void Awake()
    {
        if(isBullet) Invoke("Expire", timeToLive);
    }

    private void Expire()
    {
        Destroy(gameObject);
        return;
    }

    private void spawnParticleSystem()
    {
        GameObject p = Instantiate(particle, this.transform.position, this.transform.rotation);
        p.GetComponent<ParticleSystem>().startColor = customColor;
        p.GetComponent<ParticleSystemRenderer>().material = customShader;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "AllyBullet")
        {
            if(collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<EnemyMechanics1>().takeDamage(damage);
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

        if(this.gameObject.tag == "EnemyBullet")
        {
            if(collision.gameObject.tag == "Player")
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
            if(collision.gameObject.tag == "Player")
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
}
