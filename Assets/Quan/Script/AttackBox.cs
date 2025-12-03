using UnityEngine;

public class AttackBox : MonoBehaviour
{

 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {



            Debug.Log("Gây damage!");


            //int damage = 10;

            //// Nếu có EnemyData thì lấy damage từ đó
            //if (parentEnemy != null && parentEnemy.levelData != null)
            //    damage = parentEnemy.levelData.finalDamage;

            //// Giả sử Player có script nhận damage tên là "PlayerHealth"
            ////collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
