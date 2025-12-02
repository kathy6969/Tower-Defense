using UnityEngine;

public class TPDamage : MonoBehaviour
{
    public bool DestroyOnHit = true;
    public int damageAmount;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHP enemyHP = collision.gameObject.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDamage(damageAmount); // Deal 10 damage to the enemy
                if (DestroyOnHit)
                {
                    Destroy(gameObject); // Destroy the projectile after hitting an enemy
                }
            }
        }
    }
}
