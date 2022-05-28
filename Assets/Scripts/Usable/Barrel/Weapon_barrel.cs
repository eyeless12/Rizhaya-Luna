using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon_barrel : Prop
{
    [SerializeField] private List<GameObject> guns;
    private Transform _transform;
    private bool _spawned;
    private GameObject RandomGun => guns[Random.Range(0, guns.Count)];
    protected override void OnBulletTrigger()
    {
        if(_spawned)
            return;
        
        Instantiate(RandomGun, _itemTransform.position, Quaternion.identity);
        _spawned = true;
        
        Destroy(gameObject);
    }
}