using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Barrel : MonoBehaviour
{
    [SerializeField] private List<GameObject> guns;
    private Transform _transform;
    private bool spawned;
    
    private GameObject RandomGun => guns[Random.Range(0, guns.Count)];

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(spawned)
            return;
        
        if (!other.CompareTag("Bullet")) return;
        Debug.Log("BARREL");
        Instantiate(RandomGun, _transform.position, Quaternion.identity);
        spawned = true;
        Destroy(gameObject);
    }
}
