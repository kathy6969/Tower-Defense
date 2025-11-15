using UnityEngine;

// Dòng này cho phép bạn tạo file data từ menu Chuột phải > Create > Enemy > Stats Data
[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemy/Stats Data")]
public class EnemyStatsData : ScriptableObject
{
    [Header("Base Stats")]
    public int level = 1;
    public float baseHealth = 100f;
    public float baseDamage = 10f;

    [Header("Level-up Growth")]
    [Tooltip("Số HP cộng thêm mỗi cấp (so với cấp 1)")]
    public float healthPerLevel = 10f;
    [Tooltip("Số Dame cộng thêm mỗi cấp (so với cấp 1)")]
    public float damagePerLevel = 5f;

    // Bạn có thể thêm bất cứ thông tin gì khác ở đây
    // ví dụ:
    // public string enemyName = "Goblin";
    // public float moveSpeed = 3f;
}