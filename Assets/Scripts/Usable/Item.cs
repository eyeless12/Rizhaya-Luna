using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usable;

//Общий класс всех предметов с набором необходимых методов
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour, IItem
{
    [SerializeField] private Vector3 handsOffset;
    public bool IsTaken { get; private set; }
    public GameObject Hands { get; private set; }
    public GameObject Owner { get; private set; }
    public Vector2 OwnerLookDirection => Math.Sign(Owner.transform.rotation.y) > 0 ?
        Vector2.right : Vector2.left;
    
    protected Transform _inHandsPosition;
    protected Transform _itemTransform;
    protected Rigidbody2D _itemPhysics;
    
    public virtual void Start()
    {
        _itemTransform = GetComponent<Transform>();
        _itemPhysics = GetComponent<Rigidbody2D>();
        IsTaken = false;
    }

    public virtual void Update()
    {
        if (IsTaken && Hands)
        {
            _itemTransform.position = _inHandsPosition.position + handsOffset;
        }
    }

    public void SetOwner(GameObject hands)
    {
        Hands ??= hands;
        Owner ??= hands.transform.parent.gameObject;
        _inHandsPosition = hands.GetComponent<Transform>();
        _itemTransform.rotation = _inHandsPosition.rotation;
        _itemPhysics.freezeRotation = true;
        IsTaken = true; 
    }

    public void DiscardOwner(bool throwing)
    {
        _itemTransform.rotation = _inHandsPosition.rotation;
        _itemPhysics.freezeRotation = false;
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
        var throwVector = new Vector2(30 * velocity.x, 20 * Math.Abs(velocity.x));
        
        _itemPhysics.AddTorque(-5f * Math.Sign(OwnerLookDirection.x), ForceMode2D.Impulse);
        //_itemPhysics.velocity = throwVector;
        _itemPhysics.AddForce(throwVector, ForceMode2D.Impulse);
    }
    
    public virtual void Use()
    {
        Debug.Log($"USED {gameObject.name}");
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
