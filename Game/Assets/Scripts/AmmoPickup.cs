using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    public bool isWeapon;
    public Weapon[] weapons;
    public Weapon w;

    void Awake()
    {
        if (!isWeapon) return;
        int temp = Random.Range(0, weapons.Length);
        w = weapons[temp];
        this.GetComponent<SpriteRenderer>().sprite = w.texture;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "Ammo")
        {
            if (collision.gameObject.name != "Player") return;
            Score.UpdateScore(1);
            Shooting s = GameObject.Find("Player").GetComponent<Shooting>();
            s.Ammo += 2;
            s.updateAmmoDisplay(s.Ammo);
            FindObjectOfType<AudioManager>().Play("ItemPickup");
            Destroy(this.gameObject);
        } else if (this.gameObject.tag == "Health")
        {
            if (collision.gameObject.name != "Player") return;
            Score.UpdateScore(1);
            PlayerHealth ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
            ph.heal(1);
            Destroy(this.gameObject);
        } else if(this.gameObject.tag == "Oxygen")
        {
            if (collision.gameObject.name != "Player") return;
            Score.UpdateScore(1);
            Oxygen o = GameObject.Find("OxLevel").GetComponent<Oxygen>();
            o.fillOxygen(o.maxOxygen, 2.75f);
            FindObjectOfType<AudioManager>().Play("OxygenRegain");
            Destroy(this.gameObject);
        } else if(this.gameObject.tag == "WeaponDrop")
        {
            if (collision.gameObject.name != "Player") return;
            WeaponManager wm = collision.GetComponent<WeaponManager>();
            wm.changeWeapon(w);
            FindObjectOfType<AudioManager>().Play("ItemPickup");
            Destroy(this.gameObject);
        }
    }
}
