using System;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class Player_Movement : MonoBehaviour
{
    private float acceleration = 10;
    private Rigidbody2D rigidBodyComponent;

    private void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var v = Input.GetAxis("Vertical"); // Возвращают от -1 до 1
        var h = Input.GetAxis("Horizontal");
        var movementVector = new Vector2(h, v);
        rigidBodyComponent.velocity = movementVector * acceleration;
        // Другой вариант, без Rigidbody2D.
        // Указываются абсолютные координаты, а не смещение.
        // Перемещение происходит мгновенно, в отличие от скорости.
        // transform.position = new Vector3(...)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.name == "finish")
        {
            var rand = new Random().NextDouble() * 10;
            transform.localScale = new Vector3(10, 10, 10);
        }
    }
}