using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //current weapon
    public Weapon currentWeapon;

    //scripts to effect
    private Shooting shooting;
    private PlayerHealth ph;
    private Bullet bulletScript;

    // Start is called before the first frame update
    void Start()
    {
        //set scripts (bulletScript reference will be set when instaniating)
        shooting = this.GetComponent<Shooting>();
        ph = this.GetComponent<PlayerHealth>();

        setupWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWeapon.pointer)
        {
            LineRenderer lr = GameObject.Find("Pointer").GetComponent<LineRenderer>();
            lr.SetPosition(0, this.transform.position);
            Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetDir = (mousePos - transform.position).normalized;
            Vector3 targetPoint = mousePos + targetDir * currentWeapon.pointerLength;
            Vector3 targetPoint2 = Vector2.MoveTowards(this.transform.position, targetPoint, Mathf.Infinity);
            lr.SetPosition(1, targetPoint2);
        }
    }

    public void changeWeapon(Weapon w)
    {
        currentWeapon = w;
        setupWeapon();
    }

    public void setupWeapon()
    {
        if (currentWeapon.pointer)
        {
            GameObject pointer = GameObject.Find("Pointer");
            pointer.SetActive(true);
            LineRenderer lr = pointer.GetComponent<LineRenderer>();
            lr.startColor = currentWeapon.pointerColor;
            lr.endColor = currentWeapon.pointerColor;
            lr.material = currentWeapon.pointerMaterial;
        }
        else GameObject.Find("Pointer").SetActive(false);
    }
}
