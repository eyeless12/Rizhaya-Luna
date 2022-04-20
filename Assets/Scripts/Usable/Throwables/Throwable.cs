using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float timeToAction;
    [SerializeField] private float needsActivation;
    [SerializeField] private int projectilesCount;
    [SerializeField] private float projectilesLifetime;

    private Rigidbody2D _rb;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }
}
