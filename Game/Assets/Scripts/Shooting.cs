using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    private Transform firePoint;
    private LineLogic ll;
    public GameObject bulletPrefab;
    private GameObject currentBullet;

    public int Ammo;

    public float bulletForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        ll = GameObject.Find("Borders").GetComponent<LineLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentBullet != null && Ammo >= 1) ll.updateAnchor(currentBullet);
            else //play Sound;
        }
    }

    private void Shoot()
    {
        currentBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
