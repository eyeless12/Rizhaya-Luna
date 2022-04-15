using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool isTaken;
    private Transform _weaponTransform;
    private GameObject _owner;
    private Transform _inHandsPosition;

    private void Start()
    {
        _weaponTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (isTaken && _owner)
        {
            _weaponTransform.position = _inHandsPosition.position;
        }
    }

    public void SetOwner(GameObject owner)
    {
        _owner ??= owner;
        _inHandsPosition = owner.transform.Find("Hands").GetComponent<Transform>();
        isTaken = true;
    } 
}
