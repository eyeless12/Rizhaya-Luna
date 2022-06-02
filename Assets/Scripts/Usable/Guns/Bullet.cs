using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    
    protected Rigidbody2D _rb;
    protected Transform _transform;
    [SerializeField] private ParticleSystem impact;
    protected int _ground;

    [NonSerialized] public Vector2 direction;
    [NonSerialized] public float lifetime;
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();

        _ground = LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        _rb.velocity = direction * speed;
        if (lifetime < 0) Destroy(gameObject);
        lifetime -= Time.deltaTime;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        var target = other.gameObject;
        
        if (target.CompareTag("Player"))
            GameManager.Players.SetIGS(target, GameManager.PlayerIGS.Dead);

        if (target.CompareTag("Bullet_Collide_Block"))
        {
            direction = new Vector2(direction.x * -1, direction.y);
            return;
        }
        
        if (target.layer == _ground)
            impact.Play();
        
        Destroy(gameObject);
    }   
}
