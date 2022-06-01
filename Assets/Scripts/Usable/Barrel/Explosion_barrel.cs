using UnityEngine;

[RequireComponent(typeof(Explode))]
public class Explosion_barrel : Prop
{
    private Explode _explode;

    public override void Start()
    {
        base.Start();
        _explode = GetComponent<Explode>();
    }

    protected override void OnBulletTrigger()
    {
        _explode.Boom();
    }
}
