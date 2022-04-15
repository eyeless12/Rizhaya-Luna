using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement: MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private bool _hands;
    [SerializeField] private Transform handsTransform;
    [SerializeField] private float pickupRange = 1f;
    private Vector2 moveInput = Vector2.zero;

    private Rigidbody2D rb;
    private bool facingRight = true;

    public Transform feetPos;
    private BoxCollider2D _boxCollider;
    [SerializeField] private GameObject feet;
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
        _boxCollider = feet.GetComponent<BoxCollider2D>();
        _hands = true;
        DontDestroyOnLoad(gameObject);
    }

    private bool isGrounded => _boxCollider.IsTouchingLayers(whatIsGround.value);

    // (feetPos.position, checkRadius, whatIsGround);
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

    public void Pickup()
    {
        var inRange = Physics2D.OverlapCircle(transform.position, pickupRange, LayerMask.GetMask("Weapon"));
        if (inRange != null && _hands)
        {
            var weapon = inRange.GetComponent<Weapon>();
            var weaponTransform = inRange.GetComponent<Transform>();
            weaponTransform.SetParent(GetComponent<Transform>(), true);
            weapon.SetOwner(gameObject);    
            _hands = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}