using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{

    public float maxOxygen;
    public float currentOxygen;

    void Start()
    {
        currentOxygen = maxOxygen;
    }

    public void loseOxygen()
    {
        if(currentOxygen > 0) currentOxygen -= Time.deltaTime;
        if (currentOxygen < 0) currentOxygen = 0;
        if (currentOxygen == 0) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Die();
    }

}
