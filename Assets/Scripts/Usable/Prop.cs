using System;
using UnityEngine;

public class Prop : Item
{
    protected int Health;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
            OnBulletTrigger();
    }

    protected virtual void OnBulletTrigger()
    {
        Debug.Log("Caught bullet!");
        throw new Exception("Pure virtual method called!");
    }
}
