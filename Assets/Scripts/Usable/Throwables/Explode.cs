using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] private int projectilesCount;
    [SerializeField] private float projectilesLifetime;
    [SerializeField] private int explodeArea;
    [SerializeField] private GameObject explosion;

    private Transform _tf;
    private Collider2D _collider;
    private Throwable _throwableCharacteristics;

    private void Awake()
    {
        _tf = GetComponent<Transform>();
        _collider = GetComponent<Collider2D>();
    }

    public void Boom()
    {
        _collider.enabled = false;
        Instantiate(explosion, _tf.position, _tf.rotation);

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
        
        Destroy(gameObject);
    }
}
