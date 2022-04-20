using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(clearParticleSystems());
    }

    private IEnumerator clearParticleSystems()
    {
        while(true)
        {
            GameObject[] toClear = GameObject.FindGameObjectsWithTag("BShatter");
            foreach (GameObject g in toClear) Destroy(g);
            yield return new WaitForSeconds(20);
        }
    }
}
