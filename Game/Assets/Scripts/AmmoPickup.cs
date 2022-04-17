using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        Shooting s = GameObject.Find("Player").GetComponent<Shooting>();
        s.Ammo++;
        s.updateAmmoDisplay(s.Ammo);
        Destroy(this.gameObject);
    }
}
