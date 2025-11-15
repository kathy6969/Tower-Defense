using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class EnemyHitbox : MonoBehaviour
{
    // Sát thương sẽ được set từ EnemyAttackState
    public float damage;

    // Danh sách những đối tượng ĐÃ TRÚNG ĐÒN trong 1 lần vung
    private List<Collider2D> hitTargets;

    /// <summary>
    /// Hàm này được gọi khi GameObject (attackBox) được BẬT (SetActive(true))
    /// </summary>
    void OnEnable()
    {
        // Bắt đầu một cú đánh mới -> Xóa danh sách mục tiêu cũ
        if (hitTargets == null)
        {
            hitTargets = new List<Collider2D>();
        }
        hitTargets.Clear();
    }

    /// <summary>
    /// Hàm này chạy khi có gì đó va chạm với trigger
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Kiểm tra xem có phải là Player không (Quan trọng: Player phải có Tag "Player")
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // 2. Kiểm tra xem đã đánh trúng mục tiêu này TRONG CÙNG 1 CÚ VUNG chưa
        if (hitTargets.Contains(other))
        {
            return; // Đã đánh rồi, không đánh nữa
        }

        // 3. Gây sát thương
        // (Giả sử Player có script tên 'PlayerHealth' và hàm 'TakeDamage')
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("Hitbox đánh trúng Player, gây " + damage + " sát thương");
            playerHealth.TakeDamage(damage);

            // 4. Thêm vào danh sách đã đánh
            hitTargets.Add(other);
        }
    }

    // Quan trọng: Đảm bảo Collider2D trên object này được set là 'Is Trigger' = true
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("Collider trên " + gameObject.name + " cần được set là 'Is Trigger'!");
            col.isTrigger = true;
        }
    }
}