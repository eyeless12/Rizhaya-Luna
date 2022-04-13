using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject PF_bullet;
    [SerializeField] private int spread = 1;
    private Transform _initialBulletPoint;
    private Transform _gun;
    private Vector2 _direction;
    
    private void Awake()
    {
        _initialBulletPoint = GetComponentInChildren<Transform>();
        _gun = GetComponent<Transform>();
    }

    public void Shoot()
    {
        Instantiate(PF_bullet, _initialBulletPoint, false);
    }
}
