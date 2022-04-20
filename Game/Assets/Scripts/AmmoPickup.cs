using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "Ammo")
        {
            if (collision.gameObject.name != "Player") return;
            Shooting s = GameObject.Find("Player").GetComponent<Shooting>();
            s.Ammo++;
            s.updateAmmoDisplay(s.Ammo);
            FindObjectOfType<AudioManager>().Play("ItemPickup");
            Destroy(this.gameObject);
        } else if (this.gameObject.tag == "Health")
        {
            if (collision.gameObject.name != "Player") return;
            PlayerHealth ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
            ph.heal(1);
            //FindObjectOfType<AudioManager>().Play("Heal");
            Destroy(this.gameObject);
        } else if(this.gameObject.tag == "Oxygen")
        {
            if (collision.gameObject.name != "Player") return;
            Oxygen o = GameObject.Find("OxLevel").GetComponent<Oxygen>();
            o.fillOxygen(o.maxOxygen, 2.75f);
            FindObjectOfType<AudioManager>().Play("OxygenRegain");
            Destroy(this.gameObject);
        }
    }
}
