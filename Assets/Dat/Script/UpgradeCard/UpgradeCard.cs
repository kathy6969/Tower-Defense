using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCard", menuName = "Tower/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    [Header("UI Info")]
    public string parentID;
    public string ID;
    public string UpgradeName;
    [TextArea] public string UpgradeDescription;
    public Sprite UpgradeImage;
    [Header("Upgrade Level settings")]
    public int UpgradeLevel;
    public UpgradeCard previousLevel;

    [Header("Upgrade Type")]
    public UpgradeCardType upgradeCardType;

    [Header("Summon Settings")]
    public SummonType summonType;
    public Spitits.SpiritType spiritType; // nếu là summon spirits
    public GameObject SummonPrefab;
    [Header("Stat Upgrade Settings")]
    public TowerStatType targetStat;   // ví dụ: Range, FireRate, MaxTargets
    public float statValue;            // giá trị thay đổi (cộng thêm hoặc nhân)
    public bool isMultiplier = false;  // nếu true thì nhân, nếu false thì cộng
}
public enum UpgradeCardType
{
    Stat,
    Weapon,
    Summon
}

public enum TowerStatType
{
    Range,
    FireRate,
    MaxTargets
}
public enum SummonType
{
    Spirits,
    DragonMinion,
    ElectricOrb
}
