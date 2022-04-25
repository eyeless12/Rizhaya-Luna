using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usable;

public class Item : MonoBehaviour, IItem
{
    [SerializeField] private Vector3 handsOffset;
    public bool IsTaken { get; private set; }
    public GameObject Hands { get; private set; }
    public GameObject Owner { get; private set; }
    public Vector2 OwnerLookDirection => Math.Sign(Owner.transform.rotation.y) > 0 ?
        Vector2.right : Vector2.left;
    
    private Transform _inHandsPosition;
    private Transform _itemTransform;
    private Rigidbody2D _itemPhysics;
    
    public virtual void Start()
    {
        _itemTransform = GetComponent<Transform>();
        _itemPhysics = GetComponent<Rigidbody2D>();
        IsTaken = false;
    }

    public virtual void Update()
    {
        if (IsTaken && Hands)
            _itemTransform.position = _inHandsPosition.position + handsOffset;
    }

    public void SetOwner(GameObject hands)
    {
        Hands ??= hands;
        Owner ??= hands.transform.parent.gameObject;
        _inHandsPosition = hands.GetComponent<Transform>();
        _itemTransform.rotation = _inHandsPosition.rotation;
        IsTaken = true; 
    }

    public void DiscardOwner(bool throwing)
    {
        _itemTransform.rotation = _inHandsPosition.rotation;
        _itemTransform.parent = null;
        if (throwing) Throw();
        Hands = null;
        IsTaken = false;
    }

    public void Throw()
    {
        var velocity = Owner.GetComponent<Rigidbody2D>().velocity;
        var ownerTransform = Owner.GetComponent<Transform>();
        Debug.Log(ownerTransform.rotation.y);
        var throwVector = new Vector2(2 * velocity.x, 1 * Math.Abs(velocity.x)) ;
        _itemPhysics.velocity = throwVector;
    }

    public virtual void Use()
    {
        Debug.Log($"USED {gameObject.name}");
    }
}
