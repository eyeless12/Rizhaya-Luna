using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isTaken;
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

    public void SetOwner(GameObject owner, Player_Movement player)
    {
        _owner ??= owner;
        _inHandsPosition = owner.GetComponent<Transform>();
        _weaponTransform.rotation = _inHandsPosition.rotation;
        // if (!player.GetComponent<Player_Movement>().facingRight)
        //     _weaponTransform.Rotate(0f, 180f, 0);
        isTaken = true;
    }

    public void DiscardOwner()
    {
        _owner = null;
        isTaken = false;
        _weaponTransform.parent = null;
    }
}
