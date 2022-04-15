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
    private bool facingRight = true;

    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    private Fire inHands;

    private Animations _animationsController;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inHands = GetComponentInChildren<Fire>();
        _animationsController = GetComponent<Animations>();
        DontDestroyOnLoad(gameObject);
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
        HandleRun();
        HandleFlip();
        HandleJump();
    }

    private void HandleRun()
    {
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
        _animationsController.SetRunAnimation(moveInput.x);
    }

    private void HandleJump()
    {
        if (!isJumping) return;
        if (jumpTimeCounter <= 0)
        {
            isJumping = false;
            return;
        }
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpTimeCounter -= Time.deltaTime;
    }

    private void HandleFlip()
    {
        if (!facingRight && moveInput.x > 0)
            Flip();
        else if (facingRight && moveInput.x < 0)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
            inHands.Shoot();
    }
}