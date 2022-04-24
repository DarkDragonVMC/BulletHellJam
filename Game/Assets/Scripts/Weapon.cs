using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{

    public new string name;
    public string description;

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

    public bool shotgun;
    public int bullets;
    public float spray;

    public Vector2 fpOffset;

    public Sprite texture;
    public Sprite secondTexture;
    public string soundName;

    public GameObject Shoot(Transform firePoint)
    {
        GameObject[] currentBullets = new GameObject[bullets];

        if(!shotgun)
        {
            GameObject currentBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
            Vector2 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector2 temp = firePoint.position;
            Vector2 lookDir = mousePos - temp;
            lookDir = lookDir.normalized;
            rb.AddForce(lookDir * bulletForce, ForceMode2D.Impulse);
            FindObjectOfType<AudioManager>().Play(soundName);
            EnemyMechanics1.BulletSaves.Add(currentBullet.GetComponent<Rigidbody2D>());

            //set Bullet values
            Bullet b = currentBullet.GetComponent<Bullet>();
            b.timeToLive = this.timeToLive;
            b.damage = this.damage;
            b.explosive = this.explosive;
            b.globalExplosive = this.explosive;
            b.explosionRadius = this.explosionRadius;
            b.healPercentage = this.healPercentage;
            b.healAmount = this.healAmount;

            currentBullets[0] = currentBullet;
        } else
        {
            for (int i = bullets; i > 0; i--) {
                Vector3 offset = Vector3.zero;
                offset.x = Random.Range(-spray, spray);
                offset.y = Random.Range(-spray, spray);
                GameObject currentBullet = Instantiate(bullet, firePoint.position + offset, firePoint.rotation);

                Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
                Vector2 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
                Vector2 temp = firePoint.position + offset;
                EnemyMechanics1.BulletSaves.Add(rb);

                //rerandomize
                Vector2 offset2 = Vector2.zero;
                offset2.x = Random.Range(-spray, spray);
                offset2.y = Random.Range(-spray, spray);

                Vector2 lookDir = mousePos - temp + offset2;
                lookDir = lookDir.normalized;
                rb.AddForce(lookDir * bulletForce, ForceMode2D.Impulse);

                //set Bullet values
                Bullet b = currentBullet.GetComponent<Bullet>();
                b.timeToLive = this.timeToLive;
                b.damage = this.damage;
                b.explosive = this.explosive;
                b.globalExplosive = this.explosive;
                b.explosionRadius = this.explosionRadius;
                b.healPercentage = this.healPercentage;
                b.healAmount = this.healAmount;

                currentBullets[i - 1] = currentBullet;
            }
            FindObjectOfType<AudioManager>().Play(soundName);
        }

        return currentBullets[0];
    }

}
