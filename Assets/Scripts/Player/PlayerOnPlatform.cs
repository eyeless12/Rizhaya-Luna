using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    private GameObject _currentPlatform;
    [SerializeField] private BoxCollider2D playerCollider;

    public void Perform()
    {
        if (_currentPlatform == null) return;
        StartCoroutine(DisableCollision());
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            _currentPlatform = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            _currentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        var platformCollider = _currentPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.15f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
