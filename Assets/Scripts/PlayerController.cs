using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Status Effects
    public float moveSpeed = 5f;
    public float life = 3f;

    //Time Related
    public float timer = 0.0f;
    public float lastHit = 0.0f;

    //public Rigidbody2D rb;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    

    // Update is called once per frame
    void Update()
    {
        

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        rb.MovePosition( rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
    }


    //AKA Em colisão faça
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);

        if (col.CompareTag("Enemy") || col.CompareTag("ABullet") && (timer - lastHit) > 2.0)
        {
            lastHit = timer;
            life--;
        }
        
        //Change the spirte of the player to pulsating red for 2 seconds
    }
}
