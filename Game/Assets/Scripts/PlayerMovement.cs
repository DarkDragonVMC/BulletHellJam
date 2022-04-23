using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f;
    private Rigidbody2D rb;
    private GameObject camHolder;
    private Camera cam;
    private PlayerHealth ph;

    //Input
    private Vector2 movement;
    private Vector2 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        camHolder = GameObject.Find("CameraHolder");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        ph = this.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ph.dead) return;
        if (SceneManagement.paused)
        {
            movement.x = 0;
            movement.y = 0;
            return;
        }

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Move Camera with Player
        camHolder.transform.position = new Vector3(rb.position.x, rb.position.y, -10f);
    }

    //Physics Update
    private void FixedUpdate()
    {
        if (ph.dead) return;
        //Move Player
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
