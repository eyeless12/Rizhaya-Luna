using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [NonSerialized] public bool isTaken;
    public int Spread;
    public int SpreadWidth;
    public float BulletThresholdTime; 
    public float recoil;
    public float bulletLifetime;
    public int maxCapacity;

    private Transform _weaponTransform;
    private Transform _inHandsPosition;
    private Rigidbody2D _weaponPhysics;

    public Vector2 OwnerLookDirection => Math.Sign(Owner.transform.rotation.y) > 0 ?
        Vector2.right : Vector2.left;

    public GameObject Hands { get; private set; }
    public GameObject Owner { get; private set; }

    private void Start()
    {
        _weaponTransform = GetComponent<Transform>();
        _weaponPhysics = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isTaken && Hands)
        {
            _weaponTransform.position = _inHandsPosition.position;
        }
    }

    public void SetOwner(GameObject hands, Player_Movement player)
    {
        Hands ??= hands;
        Owner ??= hands.transform.parent.gameObject;
        _inHandsPosition = hands.GetComponent<Transform>();
        _weaponTransform.rotation = _inHandsPosition.rotation;
        isTaken = true;
    }

    public void DiscardOwner(bool thrw)
    {
        _weaponTransform.rotation = _inHandsPosition.rotation;
        _weaponTransform.parent = null;
        if (thrw) ThrowWeapon();
        Hands = null;
        isTaken = false;
    }

    private void ThrowWeapon()
    {
        var velocity = Owner.GetComponent<Rigidbody2D>().velocity;
        var ownerTransform = Owner.GetComponent<Transform>();
        Debug.Log(ownerTransform.rotation.y);
        var throwVector = new Vector2(2 * velocity.x, 1 * Math.Abs(velocity.x)) ;
        _weaponPhysics.velocity = throwVector;
    }
}
