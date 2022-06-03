using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource _audio;

    private static readonly int Triggered = Animator.StringToHash("Triggered");

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<Rigidbody2D>(out var otherPhysics);
        if (otherPhysics == null) return;
        
        otherPhysics.velocity = new Vector2(otherPhysics.velocity.x, force);
        _audio.Play();
        animator.SetTrigger(Triggered);
    }   
}
