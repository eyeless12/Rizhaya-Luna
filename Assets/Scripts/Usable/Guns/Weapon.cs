using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usable;

public class Weapon : Item
{
    public int Spread;
    public int SpreadWidth;
    public float BulletThresholdTime; 
    public float recoil;
    public float bulletLifetime;
    public int maxCapacity;

    private Fire _weaponAction;

    public override void Start()
    {
        base.Start();
        _weaponAction = GetComponent<Fire>();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        _weaponAction.Shoot();
        Debug.Log("Shoot!");
    }
}
