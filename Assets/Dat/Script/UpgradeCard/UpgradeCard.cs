using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeCard", menuName = "Tower/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    [Header("UI Info")]
    public string UpgradeID;
    public string UpgradeName;
    public int UpgradeLevel;
    [TextArea] public string UpgradeDescription;
    public Sprite UpgradeImage;

    [Header("Upgrade Type")]
    public UpgradeCardType upgradeCardType;

    [Header("Stat Upgrade Settings")]
    public TowerStatType targetStat;   // ví dụ: Range, FireRate, MaxTargets
    public float statValue;            // giá trị thay đổi (cộng thêm hoặc nhân)
    public bool isMultiplier = false;  // nếu true thì nhân, nếu false thì cộng

    [Header("Weapon / Bullet Settings")]
    public GameObject extraBulletPrefab; // thêm loại đạn phụ mới
    public GameObject MinionPrefab; // Tháp phụ để triệu hồi
    public bool replaceMainBullet = false;

    public void ApplyUpgrade(TowerShooter tower)
    {
        switch (upgradeCardType)
        {
            case UpgradeCardType.Stat:
                ApplyStatUpgrade(tower);
                break;
            case UpgradeCardType.Weapon:
                ApplyWeaponUpgrade(tower);
                break;
            case UpgradeCardType.Summon:
                ApplySummonUpgrade(tower);
                break;
        }
    }

    private void ApplyStatUpgrade(TowerShooter tower)
    {
        switch (targetStat)
        {
            case TowerStatType.Range:
                tower.range = isMultiplier ? tower.range * statValue : tower.range + statValue;
                break;
            case TowerStatType.FireRate:
                tower.fireRate = isMultiplier ? tower.fireRate * statValue : tower.fireRate + statValue;
                break;
            case TowerStatType.MaxTargets:
                tower.maxTargets = Mathf.RoundToInt(isMultiplier ? tower.maxTargets * statValue : tower.maxTargets + statValue);
                break;
        }
    }

    private void ApplyWeaponUpgrade(TowerShooter tower)
    {
        if (extraBulletPrefab == null) return;

        if (replaceMainBullet)
        {
            tower.bulletPrefab = extraBulletPrefab;
        }
        else
        {
            tower.AddExtraWeapon(extraBulletPrefab);
        }
    }

    private void ApplySummonUpgrade(TowerShooter tower)
    {
        if (MinionPrefab == null) return;
        if (tower.MinionSlot == null) return;
        GameObject minion = Instantiate(MinionPrefab, tower.MinionSlot.position, Quaternion.identity);
        minion.transform.parent = tower.MinionSlot;
    }
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
