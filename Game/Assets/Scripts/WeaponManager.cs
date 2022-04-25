using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    //current weapon
    public Weapon currentWeapon;

    //scripts to effect
    private Shooting shooting;
    private PlayerHealth ph;
    private Bullet bulletScript;

    //WeaponPickup screen references
    public float fadeSpeed;
    private CanvasGroup weaponPickup;
    private Image wpSprite;
    private TMPro.TextMeshProUGUI wpName;
    private TMPro.TextMeshProUGUI wpDesc;
    private Button wpCont;

    public float displaySize;

    public GameObject pointer;
    private GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        //set scripts (bulletScript reference will be set when instaniating)
        shooting = this.GetComponent<Shooting>();
        ph = this.GetComponent<PlayerHealth>();

        //set WeaponPickup screen references
        weaponPickup = GameObject.Find("WeaponPickup").GetComponent<CanvasGroup>();
        wpSprite = GameObject.Find("WeaponSprite").GetComponent<Image>();
        wpName = GameObject.Find("WPName").GetComponent<TMPro.TextMeshProUGUI>();
        wpDesc = GameObject.Find("WPDesc").GetComponent<TMPro.TextMeshProUGUI>();
        wpCont = GameObject.Find("WPCont").GetComponent<Button>();

        firePoint = GameObject.Find("FirePoint");

        wpCont.gameObject.SetActive(false);
        weaponPickup.gameObject.SetActive(false);

        setupWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon.pointer)
        {
            LineRenderer lr = pointer.GetComponent<LineRenderer>();
            lr.SetPosition(0, firePoint.transform.position);
            Vector3 mousePos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetDir = (mousePos - firePoint.transform.position).normalized;
            Vector3 targetPoint = mousePos + targetDir * currentWeapon.pointerLength;
            Vector3 targetPoint2 = Vector2.MoveTowards(firePoint.transform.position, targetPoint, currentWeapon.pointerLength);
            lr.SetPosition(1, targetPoint2);
        }
    }

    public void changeWeapon(Weapon w)
    {
        currentWeapon = w;
        StartCoroutine(fadeIn());
        setupWeapon();
    }

    public void setupWeapon()
    {
        if (currentWeapon.pointer)
        {
            pointer.SetActive(true);
            LineRenderer lr = pointer.GetComponent<LineRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(currentWeapon.pointerColor, 0.0f), new GradientColorKey(currentWeapon.pointerColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(1, 1.0f) }
            );
            lr.colorGradient = gradient;
            lr.material = currentWeapon.pointerMaterial;
        }
        else pointer.SetActive(false);

        //general setup
        shooting.timeBetweenShots = currentWeapon.timeBetweenShots;
        GameObject.Find("wpGraphics").GetComponent<SpriteRenderer>().sprite = currentWeapon.texture;
        firePoint.transform.localPosition = currentWeapon.fpOffset;
    }

    private IEnumerator fadeIn()
    {
        wpCont.gameObject.SetActive(true);
        weaponPickup.gameObject.SetActive(true);
        Score.scoreGo.SetActive(false);

        //EnemyMechanics1.saveBulletVelocity();
        //EnemyMechanics1.saveEnemyVelocity();
        SceneManagement.paused = true;

        //setup screen
        wpSprite.sprite = currentWeapon.texture;
        wpSprite.SetNativeSize();
        wpSprite.rectTransform.sizeDelta = new Vector2(wpSprite.rectTransform.sizeDelta.x * displaySize, wpSprite.rectTransform.sizeDelta.y * displaySize);
        wpName.text = currentWeapon.name;
        wpDesc.text = currentWeapon.description;

        while (weaponPickup.alpha < 1)
        {
            weaponPickup.alpha += Time.deltaTime * fadeSpeed;
            if (weaponPickup.alpha > 1) weaponPickup.alpha = 1;
            yield return null;
        }
        Time.timeScale = 0;
    }

    public void callFadeOut()
    {
        StartCoroutine(fadeOut());
    }

    private IEnumerator fadeOut()
    {
        Score.scoreGo.SetActive(true);
        Time.timeScale = 1;
        while (weaponPickup.alpha > 0)
        {
            weaponPickup.alpha -= Time.deltaTime * fadeSpeed;
            if (weaponPickup.alpha < 0) weaponPickup.alpha = 0;
            yield return null;
        }

        //EnemyMechanics1.loadBulletVelocity();
        //EnemyMechanics1.loadEnemyVelocity();
        SceneManagement.paused = false;
        wpCont.gameObject.SetActive(false);
        weaponPickup.gameObject.SetActive(false);
    }
}
