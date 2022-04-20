using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{

    public new string name;

    public GameObject bullet;
    public float bulletForce;
    public float timeBetweenShots;
    public float timeToLive;
    public int damage;

    public bool explosive;
    public int healPercentage;
    public bool pointer;
    public Color pointerColor;
    public Material pointerMaterial;
    public float pointerLength;

    public Sprite texture;
    public string soundName;

    public GameObject Shoot(Transform firePoint)
    {
        GameObject currentBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play(soundName);

        //set Bullet values
        Bullet b = currentBullet.GetComponent<Bullet>();
        b.timeToLive = this.timeToLive;
        b.damage = this.damage;
        b.explosive = this.explosive;
        b.healPercentage = this.healPercentage;

        return currentBullet;
    }

}
