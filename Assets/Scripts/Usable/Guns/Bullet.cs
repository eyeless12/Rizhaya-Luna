using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    
    private Rigidbody2D _rb;
    private Transform _transform;

    [NonSerialized] public Vector2 direction;
    [NonSerialized] public float lifetime;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        
        _rb.velocity = direction * speed ;
        _transform.rotation = Quaternion.Euler(direction);
    }

    private void Update()
    {
        if (lifetime < 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var target = other.gameObject;
            GameManager.Players.SetIGS(target, GameManager.PlayerIGS.Dead);
            Debug.Log($"{target.name} is dead!");
        }
        
        Destroy(gameObject);
    }
}
