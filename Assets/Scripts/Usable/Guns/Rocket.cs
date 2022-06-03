using UnityEngine;

public class Rocket : Bullet
{
    private Explode _explode;
    
    protected override void Start()
    {
        base.Start();
        _explode = GetComponent<Explode>();
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        var target = other.gameObject;
        if (target.CompareTag("Weapon") || target.CompareTag("Bullet"))
            return;
        
        _explode.Boom();
    }
}
