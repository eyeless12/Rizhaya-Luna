#nullable enable
using System;
using Unity.VisualScripting;
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
    private GameObject _hands;
    [SerializeField] private Transform handsTransform;
    [SerializeField] private float pickupRange = 1f;
    [SerializeField] private GameObject feet;
    
    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D rb;
    private bool _facingRight = true;

    public Transform feetPos;
    private BoxCollider2D _boxCollider;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpTime;
    private float _jumpTimeCounter;
    private bool _isJumping;

    private Fire handsAction;
    private Animations _animationsController;
    private PlayerOnPlatform _playerOnPlatform;
    private GameObject _overlapPicking = null!;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animationsController = GetComponent<Animations>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerOnPlatform = GetComponent<PlayerOnPlatform>();
        DontDestroyOnLoad(gameObject);
    }

    private bool isGrounded => _boxCollider.IsTouchingLayers(whatIsGround.value);
    
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
        HandleRun();
        HandleFlip();
        HandleJump();
    }

    private void HandleRun()
    {
        rb.velocity = new Vector2(_moveInput.x * speed, rb.velocity.y);
        _animationsController.SetRunAnimation(_moveInput.x);
    }

    private void HandleJump()
    {
        if (!_isJumping) return;
        if (_jumpTimeCounter <= 0)
        {
            _isJumping = false;
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        _jumpTimeCounter -= Time.deltaTime;
    }

    private void HandleFlip()
    {
        if (!_facingRight && _moveInput.x > 0)
            Flip();
        else if (_facingRight && _moveInput.x < 0)
            Flip();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void GoDownThroughPlatform()
    {
        _playerOnPlatform.Perform();
    }

    public void Shoot()
    {
        if (handsAction)
        {
            handsAction.Shoot();
        }
            
    }

    public void Pickup_Drop()
    {
        var inRange = Physics2D.OverlapCircle(transform.position, pickupRange, LayerMask.GetMask("Weapon"));
        switch ((bool) _hands)
        {
            case false:
                if (inRange != null) Pickup(inRange.gameObject);
                break;
            case true:
                Drop();
                break;
        }
    }

    private void Pickup(GameObject item)
    {
        var weapon = item.GetComponent<Weapon>();
        if (weapon.isTaken) return;
        
        var weaponTransform = item.GetComponent<Transform>();
        var owner = gameObject.transform.Find("Hands").gameObject;
        weaponTransform.SetParent(owner.GetComponent<Transform>(), true);
        weapon.SetOwner(owner, this);    
        _hands = item;
        handsAction = weapon.GetComponent<Fire>();
    }

    private void Drop()
    {
        var weapon = _hands.GetComponent<Weapon>();
        handsAction = null!;
        _hands = null!;
        weapon.DiscardOwner(_moveInput.x != 0 || rb.velocity.y != 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
    
    public void Use()
    {
        if (_overlapPicking != null)
        {
            var menu = _overlapPicking.GetComponent<ContextMenu>();
            menu.OnUse();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Usable"))
            _overlapPicking = other.gameObject;
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Usable"))
            _overlapPicking = null!;
    }
}