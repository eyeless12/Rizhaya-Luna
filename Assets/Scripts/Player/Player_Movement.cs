#nullable enable
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player_Movement: MonoBehaviour
{
    public float speed;
    public float jumpForce;
    
    private GameObject _itemInHandsObj;
    private GameObject _handsObject;
    [SerializeField] private float pickupRange = 1f;

    private Vector2 _moveInput = Vector2.zero;
    private Rigidbody2D rb;
    private bool _facingRight = true;
    
    private BoxCollider2D _boxCollider;
    
    public LayerMask whatIsGround;
    public float jumpTime;
    private float _jumpTimeCounter;
    private bool _isJumping;

    private Item handsAction;
    private Animations _animationsController;
    private PlayerOnPlatform _playerOnPlatform;
    private GameObject _overlapPicking = null!;

    public bool IsDead => !GameManager.Players.Alive.Contains(gameObject);

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animationsController = GetComponent<Animations>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _playerOnPlatform = GetComponent<PlayerOnPlatform>();
        _handsObject = gameObject.transform.Find("Hands").gameObject;

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
        HandleDead();
    }

    private void HandleRun()
    {
        rb.velocity = new Vector2(_moveInput.x * speed, rb.velocity.y);
        _animationsController.SetRunAnimation(_moveInput.x);
    }

    private void HandleDead()
    {
        _animationsController.SetDeadStatus(IsDead);
    }

    public void PerformDeadPhysics(float value)
    {
        
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
            handsAction.Use();
        }
    }

    public void Pickup_Drop()
    {
        var inRange = Physics2D.OverlapCircle(transform.position, pickupRange, LayerMask.GetMask("Items"));
        switch ((bool) _itemInHandsObj)
        {
            case false:
                if (inRange != null) Pickup(inRange.gameObject);
                break;
            case true:
                Drop();
                break;
        }
    }

    private void Pickup(GameObject candidate)
    {
        var item = candidate.GetComponent<Item>();
        if (item.IsTaken) return;
        
        var itemTransform = item.GetComponent<Transform>();
        itemTransform.SetParent(_handsObject.GetComponent<Transform>(), true);
        item.SetOwner(_handsObject);
        _itemInHandsObj = candidate;
        handsAction = item;
    }

    private void Drop()
    {
        var item = _itemInHandsObj.GetComponent<Item>();
        handsAction = null!;
        _itemInHandsObj = null!;
        item.DiscardOwner(_moveInput.x != 0 || rb.velocity.y != 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
    
    public void Use()
    {
        if (_overlapPicking == null) return;
        var menu = _overlapPicking.GetComponent<ContextMenu>();
        menu.OnUse();
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