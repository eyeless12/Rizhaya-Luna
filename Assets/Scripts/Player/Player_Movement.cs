#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement: MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private Vector2 _moveInput = Vector2.zero;

    private Rigidbody2D rb;
    private bool _facingRight = true;

    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    private float _jumpTimeCounter;
    private bool _isJumping;

    private Fire inHands;

    private Animations _animationsController;
    private GameObject _overlapPicking = null!;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inHands = GetComponentInChildren<Fire>();
        _animationsController = GetComponent<Animations>();
        DontDestroyOnLoad(gameObject);
    }

    private bool isGrounded => Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

    public void Move(Vector2 move)
    {
        _moveInput = move;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            _isJumping = true;
            _jumpTimeCounter = jumpTime;
        }

        if (context.canceled)
            _isJumping = false;
    }
    public void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveInput.x * speed, rb.velocity.y);
        _animationsController.SetRunAnimation(_moveInput.x);
        
        if (!_facingRight && _moveInput.x > 0)
            Flip();
        else if (_facingRight && _moveInput.x < 0)
            Flip();
        
        if (_isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce) ;
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
                _isJumping = false;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
            inHands.Shoot();
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.performed && _overlapPicking != null)
        {
            var menu = _overlapPicking.GetComponent<ContextMenu>();
            menu.OnUse();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _overlapPicking = other.gameObject;
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        _overlapPicking = null!;
    }
}