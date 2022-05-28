using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damagable : Prop
{
    [SerializeField] protected int health;

    protected override void Update()
    {
        base.Update();
        
        if (health < 0)
            Destruction();
    }

    private void Destruction()
    {
        Destroy(gameObject);
    }

    protected override void OnBulletTrigger()
    {
        health -= 10;
    }
}
