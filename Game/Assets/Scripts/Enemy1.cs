using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public Rigidbody2D rb;
    public float posX;
    public float posY;
    public int XorY;
    public float speed;
    public bool StartTriggered = false;
    public bool cycleCompleted = false;






    private void Awake()
    {
        posX = this.transform.position.x;
        posY = this.transform.position.y;

    }
    private void Start()
    {
        Randomize();
    }


    private void Update()
    {
        
            Move();
            
    }

    public void setToFalse()
    {
        StartTriggered = false;
        cycleCompleted = false;
    }
    public void triggerStart()
    {
        StartTriggered = true;
    }
    public void completeCycle()
    {
        cycleCompleted = true;
    }
    public void teleportBack()
    {
        rb.transform.position = new Vector3(posX, posY, 0);
    }

    public void Move()
    {
        
        Vector2 MoveX = new Vector2(speed, 0);
        Vector2 MoveY = new Vector2(0, speed);
        if(XorY == 0)
        {

            if (this.transform.position.x == posX && StartTriggered == false)
            {
                rb.velocity = MoveX;
                Invoke("triggerStart", 0.5f);
                                
            }
            if(this.transform.position.x >= posX + 5)
            {
                rb.velocity = -MoveX;
            }
            if (this.transform.position.x <= posX - 5)
            {
                rb.velocity = MoveX;
                
            }
            if (this.transform.position.x >= posX - 0.1f && this.transform.position.x <= posX + 0.1f && StartTriggered && !cycleCompleted)
            {
                Invoke("completeCycle", 0.5f);
            }
            if (this.transform.position.x <= posX + 0.1f && this.transform.position.x >= posX - 0.1f && cycleCompleted && StartTriggered)
            {
                rb.velocity = new Vector2(0, 0);
                teleportBack();
                Randomize();
                setToFalse();
            }

        }
        if (XorY == 1)
        {

            if (this.transform.position.y == posY && StartTriggered == false)
            {
                rb.velocity = MoveY;
                Invoke("triggerStart", 0.5f);

            }
            if (this.transform.position.y >= posY + 5)
            {
                rb.velocity = -MoveY;
            }
            if (this.transform.position.y<= posY - 5)
            {
                rb.velocity = MoveY;

            }
            if (this.transform.position.y >= posY - 0.1f && this.transform.position.y <= posY + 0.1f && StartTriggered && !cycleCompleted)
            {
                Invoke("completeCycle", 0.5f);
            }
            if (this.transform.position.y <= posY + 0.1f && this.transform.position.y >= posY - 0.1f && cycleCompleted && StartTriggered)
            {
                rb.velocity = new Vector2(0, 0);
                teleportBack();
                Randomize();
                setToFalse();
            }
        }
    }
    public void Randomize()
    {
        XorY = Random.Range(0, 2);
        Debug.Log(XorY);
    }
    
}
