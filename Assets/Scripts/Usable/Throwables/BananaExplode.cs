using UnityEngine;

public class BananaExplode : Explode
{
    [SerializeField] private GameObject shrapnel;
    [SerializeField] private float speed;

    [HideInInspector] public bool isCassette;

    private LayerMask _ground;

    private void Awake()
    {
        _tf = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
        _ground = LayerMask.NameToLayer("Ground");
    }
    
    public override void Boom()
    {
        if (_exploded) return;
        
        _exploded = true;
        Instantiate(explosion, _tf.position, _tf.rotation);

        if (isCassette)
        {
            base.Boom();
            return;
        }
        
        foreach (var direction in Utils.GenerateDirections(
            projectilesCount,
            explodeArea,
            1, Vector3.up))
        {
            var cassette = Instantiate(
                shrapnel,
                _tf.position,
                _tf.rotation);
            cassette.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
            cassette.GetComponent<BananaExplode>().isCassette = true;
        }
        
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("BANANA");
        if (!isCassette) return;
        
        if (other.gameObject.CompareTag("Props")
            || other.gameObject.layer == _ground)
        {
            base.Boom();
        }
    }
}
