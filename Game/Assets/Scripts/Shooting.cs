using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    private Transform firePoint;
    private LineLogic ll;
    public GameObject bulletPrefab;
    private GameObject currentBullet;
    private TMPro.TextMeshProUGUI ammoDisplay;
    private PlayerHealth ph;
    private WeaponManager wm;

    public int Ammo = 10;

    public float bulletForce = 20f;

    public float timeBetweenShots;
    private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = GameObject.Find("FirePoint").GetComponent<Transform>();
        ll = GameObject.Find("Borders").GetComponent<LineLogic>();
        ammoDisplay = GameObject.Find("ammoDisplay").GetComponent<TMPro.TextMeshProUGUI>();
        ph = this.GetComponent<PlayerHealth>();
        wm = this.GetComponent<WeaponManager>();
        updateAmmoDisplay(Ammo);

        cooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ph.dead) return;
        if (SceneManagement.paused) return;
        if (cooldown > 0) cooldown = cooldown - Time.deltaTime;
        if (cooldown < 0) cooldown = 0;

        if (Input.GetButton("Fire1") && cooldown == 0) Shoot();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentBullet != null && Ammo >= 1)
            {
                Ammo--;
                updateAmmoDisplay(Ammo);
                ll.updateAnchor(currentBullet);
            }
            //else Play Sound
        }
    }

    private void Shoot()
    {
        currentBullet = wm.currentWeapon.Shoot(firePoint);
        cooldown = timeBetweenShots;
    }

    public void updateAmmoDisplay(int newNumber)
    {
        ammoDisplay.SetText(newNumber.ToString());
    }
}