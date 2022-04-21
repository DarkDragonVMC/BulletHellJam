using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPController : MonoBehaviour
{

    public static PPController instance;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
