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
    public float explosionRadius;
    public int healPercentage;
    public int healAmount;
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
        Vector2 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        Vector2 temp = firePoint.position;
        Vector2 lookDir = mousePos - temp;
        rb.AddForce(lookDir * bulletForce, ForceMode2D.Impulse);
        FindObjectOfType<AudioManager>().Play(soundName);

        //set Bullet values
        Bullet b = currentBullet.GetComponent<Bullet>();
        b.timeToLive = this.timeToLive;
        b.damage = this.damage;
        b.explosive = this.explosive;
        b.explosionRadius = this.explosionRadius;
        b.healPercentage = this.healPercentage;
        b.healAmount = this.healAmount;

        return currentBullet;
    }

}
