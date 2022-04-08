using System;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class Player_Movement: MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;
    private bool facingRight = false;

    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;
    

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool isGrounded => Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

    public void Move(Vector2 move)
    {
        moveInput = move;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            isJumping = false;
    }

    public void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        
        if (!facingRight && moveInput > 0)
            Flip();
        else if (facingRight && moveInput < 0)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}