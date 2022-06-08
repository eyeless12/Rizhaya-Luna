using UnityEngine;

public class BananaExplode : Explode
{
    [SerializeField] private GameObject shrapnel;
    [SerializeField] private float speed;

    [HideInInspector] public bool isCassette;

    private LayerMask _ground;

    protected override void Awake()
    {
        base.Awake();
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

    public override void Activate()
    {
        Debug.Log("Not Implemented");
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!isCassette) return;
        
        if (other.gameObject.CompareTag("Props")
            || other.gameObject.layer == _ground)
        {
            base.Boom();
        }
    }
}
