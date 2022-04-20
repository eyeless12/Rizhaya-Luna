using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Barrel : MonoBehaviour
{
    [SerializeField] private List<GameObject> guns;
    private Transform _transform;
    private bool spawned;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(spawned)
            return;
        if (!other.CompareTag("Bullet")) return;
        Instantiate(RandomGun(), _transform.position, Quaternion.identity);
        spawned = true;
        Destroy(gameObject);

    }

    private GameObject RandomGun()
    {
        return guns[Random.CreateFromIndex(1111).NextInt(guns.Count)];
    }
}
