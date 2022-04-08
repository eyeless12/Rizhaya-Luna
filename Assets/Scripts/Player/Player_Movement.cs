using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement: MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private Vector2 moveInput = Vector2.zero;

    private Rigidbody2D rb;
    //private CharacterController controller;
    private bool facingRight = false;

    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //controller = GetComponent<CharacterController>();
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
        }

        if (context.canceled)
            isJumping = false;
    }
    public void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        
        if (!facingRight && moveInput.x > 0)
            Flip();
        else if (facingRight && moveInput.x < 0)
            Flip();
        
        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce) ;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                isJumping = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}