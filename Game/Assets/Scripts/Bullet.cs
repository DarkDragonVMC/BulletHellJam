using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float timeToLive;
    public int damage;
    public bool isBullet;

    void Awake()
    {
        if(isBullet) Invoke("Expire", timeToLive);
    }

    private void Expire()
    {
        Destroy(gameObject);
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "AllyBullet")
        {
            if(collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<EnemyMechanics1>().takeDamage(damage);
                Destroy(this.gameObject);
            }
            if (collision.gameObject.tag == "Enemy3")
            {
                collision.GetComponent<EnemyMechanics3>().takeDamage(damage);
                Destroy(this.gameObject);
            }
        }

        if(this.gameObject.tag == "EnemyBullet")
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
                Destroy(this.gameObject);
            }
        }

        if(this.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerHealth>().takeDamage(damage);
            } 
        }
    }
}
