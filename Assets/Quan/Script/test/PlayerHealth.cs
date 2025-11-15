using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    private bool isDead = false;

    void Start()
    {
        // Bắt đầu game với đầy máu
        currentHealth = maxHealth;
        isDead = false;
        Debug.Log("Player đã sẵn sàng với " + currentHealth + " HP.");
    }

    /// <summary>
    /// Đây là hàm mà EnemyHitbox sẽ gọi
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        // Nếu đã chết, không nhận thêm sát thương
        if (isDead)
        {
            return;
        }

        // Trừ máu
        currentHealth -= damageAmount;

        // In ra thông báo để bạn biết nó hoạt động
        Debug.LogWarning("!!! PLAYER bị đánh: -" + damageAmount + " HP. Máu còn: " + currentHealth);

        // Kiểm tra xem đã chết chưa
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Tránh máu âm
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.LogError("GAME OVER: Player đã chết!");

        // Bạn có thể thêm logic chết ở đây
        // Ví dụ: Tắt script điều khiển của Player
        // GetComponent<PlayerMovement>().enabled = false;

        // Hoặc bật animation chết
        // Animator anim = GetComponent<Animator>();
        // if (anim != null) anim.Play("Player_Die");
    }

    // Hàm này để test hồi máu (nếu cần)
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player hồi máu! HP hiện tại: " + currentHealth);
    }
}