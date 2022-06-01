using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

//Класс кидаемого оружия
public class Throwable : Item
{
    [SerializeField] private float timeToAction;
    [SerializeField] private bool needsActivation;
    [SerializeField] private Animator _animator;
    
    private bool _activated;
    private float _time;
    private Explode _throwableAction; 

    private Rigidbody2D _rb;
    private Transform _transform;
    private static readonly int Activated = Animator.StringToHash("Activated");

    public override void Start()
    {
        base.Start();
        _throwableAction = GetComponent<Explode>();
    }

    protected override void Update()
    {
        base.Update();
        if (!_activated) return;
        
        _animator.SetTrigger(Activated);
        _time += Time.deltaTime;
        if (_time < timeToAction) return;
        
        _throwableAction.Boom();
    }

    public override void Use()
    {
        _activated = true;
    }
}
