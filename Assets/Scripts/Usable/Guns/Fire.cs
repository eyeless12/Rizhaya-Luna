using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] private Transform initialBulletPoint;
    private Weapon _weaponCharacteristics;
    private Transform _gun;
    
    private float _shootTime;
    private bool _canShoot = true;
    private int _magazine;

    private void Awake()
    {
        _weaponCharacteristics = GetComponent<Weapon>();
        _gun = GetComponent<Transform>();
        _magazine = _weaponCharacteristics.maxCapacity;
    }

    public void Shoot()
    {
        if (!_canShoot || _magazine <= 0) return;

        foreach (var direction in GenerateDirections())
        {
            var bullet = Instantiate(pf_bullet, initialBulletPoint.position, initialBulletPoint.rotation)
                .GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.lifetime = _weaponCharacteristics.bulletLifetime;
        }
        
        PerformRecoil();
        _canShoot = false;
        _shootTime = _weaponCharacteristics.BulletThresholdTime;
        _magazine -= 1;
    }

    public void FixedUpdate()
    {
        if (!_canShoot)
            _shootTime -= Time.deltaTime;

        if (_shootTime < 0)
            _canShoot = true;
    }

    private IEnumerable<Vector3> GenerateDirections()
    {
        var ways = _weaponCharacteristics.Spread;
        if (ways % 2 != 0)
        {
            yield return _weaponCharacteristics.OwnerLookDirection;
            ways -= 1;
        }

        var angle = _weaponCharacteristics.SpreadWidth / ways;
        for (var i = 1; i <= ways / 2; i++)
        {
            var random = new Random();
            var offset = (float)random.NextDouble() * 10;
            Debug.Log(offset);
            yield return Quaternion.Euler(0, 0, angle * i)
                         * _weaponCharacteristics.OwnerLookDirection;
            yield return Quaternion.Euler(0, 0, -angle * i)
                         * _weaponCharacteristics.OwnerLookDirection;
        }
            
        
    }

    private void PerformRecoil()
    {
        var owner = _weaponCharacteristics.Owner;
        var ownerPhysics = owner.GetComponent<Rigidbody2D>();
        var recoilVector = new Vector2(
             _weaponCharacteristics.recoil * _weaponCharacteristics.OwnerLookDirection.x * -1 , _weaponCharacteristics.recoil / 5);
        Debug.Log(recoilVector);
        ownerPhysics.AddForce(recoilVector, ForceMode2D.Force);
    }
}
