using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    private Rigidbody2D rb;
    public Vector2 direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        Destroy(gameObject);
    }
}
