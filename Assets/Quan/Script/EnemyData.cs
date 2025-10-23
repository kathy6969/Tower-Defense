using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Thông tin mặc định của quái (Level 1)")]
    public string enemyName = "Quai";
    public int baseHP = 100;
    public int baseDamage = 5;
}
