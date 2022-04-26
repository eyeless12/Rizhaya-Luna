using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] private int projectilesCount;
    [SerializeField] private float projectilesLifetime;
    [SerializeField] private int explodeArea;

    private Throwable _throwableCharacteristics;
    
    public void Boom()
    {
        foreach (var direction in Utils.GenerateDirections(
            projectilesCount,
            explodeArea,
            1, Vector3.up))
        {
            var bullet = Instantiate(pf_bullet, transform.position,
                transform.rotation).GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.lifetime = projectilesLifetime;
        }
        
        Destroy(gameObject);
    }
}
