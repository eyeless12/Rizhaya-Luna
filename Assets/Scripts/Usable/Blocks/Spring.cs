using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Animator animator;
    
    private static readonly int Triggered = Animator.StringToHash("Triggered");

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<Rigidbody2D>(out var otherPhysics);
        if (otherPhysics == null) return;
        
        otherPhysics.velocity = new Vector2(otherPhysics.velocity.x, force);
        animator.SetTrigger(Triggered);
    }   
}
