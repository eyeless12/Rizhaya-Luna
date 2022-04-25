using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Throwable : Item
{
    [SerializeField] private float timeToAction;
    [SerializeField] private bool needsActivation;
    
    private bool _activated;
    private float _time;
    private Explode _throwableAction;

    private Rigidbody2D _rb;
    private Transform _transform;

    public override void Start()
    {
        base.Start();
        _throwableAction = GetComponent<Explode>();
    }

    public override void Update()
    {
        base.Update();
        if (!_activated) return;
        
        _time += Time.deltaTime;
        if (_time < timeToAction) return;
        
        _throwableAction.Boom();
        Destroy(gameObject);
    }

    public override void Use()
    {
        _activated = true;
    }
}
