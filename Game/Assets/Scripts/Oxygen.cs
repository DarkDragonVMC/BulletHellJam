using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{

    public float maxOxygen;
    public float currentOxygen;
    public bool inside;

    private Slider oxDisplay;

    void Start()
    {
        oxDisplay = this.GetComponent<Slider>();
        currentOxygen = maxOxygen;
    }

    void Update()
    {
        changeOxygen(inside);
    }

    public void changeOxygen(bool inside)
    {
        if (currentOxygen > 0 && !inside) currentOxygen -= Time.deltaTime;
        if (currentOxygen < 0) currentOxygen = 0;
        if (currentOxygen == 0) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Die();

        if (currentOxygen < maxOxygen && inside) currentOxygen += Time.deltaTime * 2.5f;
        if (currentOxygen > maxOxygen) currentOxygen = maxOxygen;

        oxDisplay.value = currentOxygen / maxOxygen;
    }

    public void fillOxygen(float value, float speed)
    {
        StartCoroutine(setOxygen(value, speed));
    }

    private IEnumerator setOxygen(float value, float speed)
    {
        if (value > maxOxygen) value = maxOxygen;
        if (value < 0) value = 0;

        if (speed <= 1) speed = 1.1f;

        while(currentOxygen < value)
        {
            Debug.Log("WUHU");
            currentOxygen += Time.deltaTime * speed;
            oxDisplay.value = currentOxygen / maxOxygen;
            yield return null;
        }

        currentOxygen = value;
    }

}
