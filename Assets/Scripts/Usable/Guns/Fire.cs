using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject pf_bullet;
    [SerializeField] private int spread = 1;
    [SerializeField] private Transform initialBulletPoint;
    private Transform _gun;
    private Vector2 _direction;
    
    private void Awake()
    {
        // _initialBulletPoint = GetComponentInChildren<Transform>();
        _gun = GetComponent<Transform>();
    }

    public void Shoot()
    {
        Instantiate(pf_bullet, initialBulletPoint.position, initialBulletPoint.rotation);
    }
}
