using System;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class Player_Movement: MonoBehaviour
{
    private float acceleration = 10;
    public float buttonTime = 0.5f;
    public float jumpHeight = 300;
    public float cancelRate = 100;
    float jumpTime;
    bool jumping;
    bool spacePressed;
    bool jumpCancelled;
    private Rigidbody2D rigidBodyComponent;

    private void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //var v = Input.GetAxis("Vertical"); // Возвращают от -1 до 1
        var h = Input.GetAxis("Horizontal");
        var movementVector = new Vector2(h, 0);
        rigidBodyComponent.velocity = movementVector * acceleration;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "finish")
        {
            var rand = new Random().NextDouble() * 10;
            transform.localScale = new Vector3(10, 10, 10);
        }
    }

    private void FixedUpdate()
    {
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpCancelled = true;
            }
            if (jumpTime > buttonTime)
            {
                jumping = false;
            }
        }
        
        if (spacePressed)
            Jump();
        
        if (jumpCancelled && jumping && rigidBodyComponent.velocity.y >= 0)
        {
            Debug.Log("here");
            rigidBodyComponent.AddForce(Vector2.down * cancelRate);
        }
    }

    private void Jump()
    {
        float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rigidBodyComponent.gravityScale));
        rigidBodyComponent.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumping = true;
        jumpCancelled = false;
        jumpTime = 0;
    }
}