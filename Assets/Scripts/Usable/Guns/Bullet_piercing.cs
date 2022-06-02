using UnityEngine;

public class Bullet_piercing : Bullet
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        var target = other.gameObject;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.collider);
        
        if (target.CompareTag("Player"))
            GameManager.Players.SetIGS(target, GameManager.PlayerIGS.Dead);
        
        if (target.CompareTag("Bullet_Collide_Block"))
        {
            direction = new Vector2(direction.x * -1, direction.y);
            return;
        }
        
        if (target.layer == _ground)
            Destroy(gameObject);
    }
}
