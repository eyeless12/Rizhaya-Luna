using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isTaken;
    private Transform _weaponTransform;
    private GameObject _owner;
    private Transform _inHandsPosition;
    private Rigidbody2D _weaponPhysics;

    private void Start()
    {
        _weaponTransform = GetComponent<Transform>();
        _weaponPhysics = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isTaken && _owner)
        {
            _weaponTransform.position = _inHandsPosition.position;
        }
    }

    public void SetOwner(GameObject owner, Player_Movement player)
    {
        _owner ??= owner;
        _inHandsPosition = owner.GetComponent<Transform>();
        _weaponTransform.rotation = _inHandsPosition.rotation;
        // if (!player.GetComponent<Player_Movement>().facingRight)
        //     _weaponTransform.Rotate(0f, 180f, 0);
        isTaken = true;
    }

    public void DiscardOwner(bool thrw)
    {
        _weaponTransform.rotation = _inHandsPosition.rotation;
        _weaponTransform.parent = null;
        if (thrw) ThrowWeapon();
        _owner = null;
        isTaken = false;
    }

    private void ThrowWeapon()
    {
        Debug.Log("Throw");
        var owner = _owner.transform.parent.gameObject;
        var velocity = owner.GetComponent<Rigidbody2D>().velocity;
        var ownerTransform = owner.GetComponent<Transform>();
        Debug.Log(ownerTransform.rotation.y);
        var throwVector = new Vector2(1 * velocity.x, 1 * Math.Abs(velocity.x)) ;
        _weaponPhysics.velocity = throwVector;
    }

}
