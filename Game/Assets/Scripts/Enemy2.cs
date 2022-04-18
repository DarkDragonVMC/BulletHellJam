using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public int UpDownLeftRight;
    public int timerSpeed = 1;
    public float timer;
    public Rigidbody2D rb;
    public int speed;
    

    public void Update()
    {
        timer = timer + timerSpeed * Time.deltaTime;
        if(timer > 2)
        {
            timer = 0;
        }
        Move();
    }
    public void Move()
    {
        Vector2 upDown = new Vector2(0, speed);
        Vector2 leftRight = new Vector2(speed, 0);
        if (timer == 0)
        {
            Randomize();
        }
        if(UpDownLeftRight == 0)
        {
            rb.velocity = -upDown;
        }
        if (UpDownLeftRight == 1)
        {
            rb.velocity = -leftRight;
        }
        if (UpDownLeftRight == 2)
        {
            rb.velocity = upDown;
        }
        if (UpDownLeftRight == 3)
        {
            rb.velocity = leftRight;
        }
    }
    public void Randomize()
    {
        UpDownLeftRight = Random.Range(0, 4);
    }

}
