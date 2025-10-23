using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Dữ liệu quái")]
    public EnemyLevelUp levelData;
    public Slider hpBar;

    private int currentHP;

    private void Start()
    {
        if (levelData == null)
        {
            Debug.LogWarning("Thiếu LevelUpData!");
            return;
        }

        // Tính toán chỉ số dựa theo cấp
        levelData.CalculateStats();

        // Gán giá trị HP hiện tại
        currentHP = levelData.finalHP;

        if (hpBar != null)
        {
            hpBar.maxValue = levelData.finalHP;
            hpBar.value = currentHP;
        }

        Debug.Log($"[{levelData.enemyName}] Lv.{levelData.currentLevel} | HP: {levelData.finalHP} | Damage: {levelData.finalDamage}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (hpBar != null)
            hpBar.value = currentHP;

        if (currentHP <= 0)
            Destroy(gameObject);
    }
}
