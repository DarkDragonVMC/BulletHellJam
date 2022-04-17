using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float timeToLive;

    void Awake()
    {
        Invoke("Expire", timeToLive);
    }

    private void Expire()
    {
        Destroy(gameObject);
        return;
    }
}
