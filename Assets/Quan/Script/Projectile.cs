using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Cấu hình Đạn")]
    public float speed = 10f;       // Tốc độ bay
    public float lifetime = 3f;     // Thời gian tồn tại (3 giây)
    [Tooltip("Nếu sprite đạn của bạn mặc định 'nhìn lên trên', hãy để là -90.")]
    public float rotationOffset = 0f; // 👈 BIẾN MỚI để xoay sprite cho đúng

    private Rigidbody2D rb;
    private Transform playerTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ----- LOGIC BAY MỚI (BAY VỀ HƯỚNG PLAYER) -----

        // 1. Tìm Player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            // 2. Tính toán hướng
            // (Vị trí Player - Vị trí của đạn) -> chuẩn hóa
            Vector2 direction = (playerObject.transform.position - transform.position).normalized;

            // 3. Đặt vận tốc
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }

            // 4. Xoay sprite của đạn để nó "nhìn" về hướng bay
            // (Atan2 nhận y trước, x sau)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
        else
        {
            // Nếu không tìm thấy Player (ví dụ Player chết), cứ bắn thẳng
            Debug.LogWarning("Projectile: Không tìm thấy Player, bắn thẳng!");
            if (rb != null)
            {
                rb.linearVelocity = transform.right * speed;
            }
        }
        // ---------------------------------------------

        // Tự hủy sau 'lifetime' (3 giây)
        Destroy(gameObject, lifetime);
    }

    // (Hàm OnTriggerEnter2D giữ nguyên, không cần sửa)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // (Code gây sát thương)
            Debug.Log("Đạn trúng Player!");
            Destroy(gameObject);
        }

        // (Tùy chọn: Hủy đạn khi đụng tường)
        // if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        // {
        //     Destroy(gameObject);
        // }
    }
}