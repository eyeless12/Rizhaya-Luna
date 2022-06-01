using System;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] protected int projectilesCount;
    [SerializeField] private float projectilesLifetime;
    [SerializeField] protected int explodeArea;
    [SerializeField] protected GameObject explosion;
    [SerializeField] protected AudioClip explosionSound;
    [SerializeField] protected GameObject afterDeath;

    protected Transform _tf;
    protected bool _exploded;
    protected Collider2D _collider;
    private Throwable _throwableCharacteristics;

    private void Awake()
    {
        _tf = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
    }

    public virtual void Boom()
    {
        if (_exploded) return;
        
        _collider.enabled = false;
        _exploded = true;
        Instantiate(explosion, _tf.position, _tf.rotation);
        GameManager.CameraShake.ActivateShake(.5f, .5f);

        foreach (var direction in Utils.GenerateDirections(
            projectilesCount,
            explodeArea,
            1, Vector3.up))
        {
            var bullet = Instantiate(pf_bullet, _tf.position,
                _tf.rotation).GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.lifetime = projectilesLifetime;
        }
        
        PerformSound();
        Destroy(gameObject);
    }

    protected void PerformSound()
    {
        var death = Instantiate(afterDeath, transform.position, transform.rotation);
        var sound = death.GetComponent<AudioSource>();
        sound.clip = explosionSound;
        sound.Play();
    }
}
