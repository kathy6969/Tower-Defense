using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLevelUp", menuName = "Data/Enemy Level Up")]
public class EnemyLevelUp : ScriptableObject
{
    [Header("Tham chiếu tới khung dữ liệu gốc của quái")]
    public EnemyData baseData;

    [Header("Cấp độ hiện tại")]
    public int currentLevel = 1;

    [Header("Chỉ số cộng thêm mỗi cấp")]
    public int hpPerLevel = 20;
    public int damagePerLevel = 5;

    [Header("Kết quả cuối cùng (tính từ baseData + cấp độ)")]
     public string enemyName;
     public int finalHP;
     public int finalDamage;

    public void CalculateStats()
    {
        if (baseData == null)
        {
            Debug.LogWarning("Thiếu EnemyData gốc!");
            return;
        }

        enemyName = baseData.enemyName;
        finalHP = baseData.baseHP + (currentLevel - 1) * hpPerLevel;
        finalDamage = baseData.baseDamage + (currentLevel - 1) * damagePerLevel;
    }
}
