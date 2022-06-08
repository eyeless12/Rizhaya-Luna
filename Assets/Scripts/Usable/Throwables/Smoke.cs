using UnityEngine;

public class Smoke : Explode
{
    [SerializeField] private ParticleSystem smokeActivated;
    [SerializeField] private ParticleSystem smoke;

    private bool _activated;

    public override void Activate()
    {
        if (_activated) return;
        
        smokeActivated.Play();
        _activated = true;
    }

    public override void Boom()
    {
        smokeActivated.Stop();

        Instantiate(smoke, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
