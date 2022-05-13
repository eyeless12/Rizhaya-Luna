using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlatform : MonoBehaviour
{
    private readonly HashSet<GameObject> _currentPlatforms = new ();
    [SerializeField] private BoxCollider2D playerCollider;

    public void Perform()
    {
        if (_currentPlatforms. Count == 0) return;
        foreach (var platform in _currentPlatforms)
            StartCoroutine(DisableCollision(platform));
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            _currentPlatforms.Add(other.gameObject);
            Debug.Log("PLATFORM ENTERED");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            _currentPlatforms.Remove(other.gameObject);
        }
    }

    private IEnumerator DisableCollision(GameObject platform)
    {
        if (platform == null) yield break;
        
        var platformCollider = platform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        yield return new WaitForSeconds(0.15f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        Debug.Log("PLATFORM DISABLED");
    }
}
