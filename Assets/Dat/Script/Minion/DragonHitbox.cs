using UnityEngine;

public class DragonHitbox : MonoBehaviour
{
    public int damage = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
