using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    
    private Rigidbody2D _rb;
    private Transform _transform;
    private int _ground;

    [NonSerialized] public Vector2 direction;
    [NonSerialized] public float lifetime;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        _ground = LayerMask.NameToLayer("Ground");
        
        _rb.velocity = direction * speed ;
        _transform.rotation = Quaternion.Euler(direction);
    }

    private void Update()
    {
        if (lifetime < 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var target = other.gameObject;
        
        if (target.CompareTag("Player"))
            GameManager.Players.SetIGS(target, GameManager.PlayerIGS.Dead); 
        
        Destroy(gameObject);
    }   
}
