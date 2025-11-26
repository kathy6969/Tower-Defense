using UnityEngine;
public class UpgradeApplier : MonoBehaviour
{
    [HideInInspector]public UpgradeCard upgradeCard;
    [HideInInspector]public GameObject Tower;
    [HideInInspector]public TowerShooter towerShooter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ApplyUpgrade(UpgradeCard Card, GameObject tower)
    {
        upgradeCard = Card;
        Tower = tower;
        // Áp dụng nâng cấp dựa trên loại thẻ
        switch (upgradeCard.upgradeCardType)
        {
            case UpgradeCardType.Stat:
                ApplyStatUpgrade(upgradeCard, Tower);
                break;
            case UpgradeCardType.Weapon:
                break;
            case UpgradeCardType.Summon:
                ApplySummonUpgrade(upgradeCard, Tower);
                break;
            default:
                Debug.LogWarning("Loại thẻ nâng cấp không xác định!");
                break;
        }
    }
    private void ApplyStatUpgrade(UpgradeCard card, GameObject tower)
    {
        TowerShooter towerShooter = tower.GetComponent<TowerShooter>();
        if (towerShooter == null)
        {
            UnityEngine.Debug.LogWarning("Tower không có component TowerShooter!");
            return;
        }
        // Thực hiện nâng cấp thuộc tính tháp dựa trên card.targetStat và card.statValue
        switch (card.targetStat)
        {
            case TowerStatType.Range:
                // Nâng cấp tầm bắn
                towerShooter.range = card.isMultiplier ? towerShooter.range * card.statValue :
                    towerShooter.range + card.statValue;
                break;
            case TowerStatType.FireRate:
                // Nâng cấp tốc độ bắn
                towerShooter.fireRate = card.isMultiplier ? towerShooter.fireRate * card.statValue :
                    towerShooter.fireRate + card.statValue;
                break;
            case TowerStatType.MaxTargets:
                // Nâng cấp số mục tiêu tối đa
                towerShooter.maxTargets = (int)(card.isMultiplier ? towerShooter.maxTargets * card.statValue :
                    towerShooter.maxTargets + card.statValue);
                break;
            default:
                Debug.LogWarning("Thuộc tính tháp không xác định!");
                break;
        }
    }
    private void ApplySummonUpgrade(UpgradeCard card, GameObject tower)
    {
        // Triệu hồi linh hồn hoặc minion dựa trên card.summonType
        switch (card.summonType)
        {
            case SummonType.Spirits:
                // Triệu hồi linh hồn
                SpiritsParent spiritsParent = tower.GetComponentInChildren<SpiritsParent>();
                if (spiritsParent == null)
                {
                    UnityEngine.Debug.LogWarning("Tower không có component SpiritsParent!");
                    return;
                }
                if (card.UpgradeLevel == 1)
                {
                    SummonSpirits(card, tower);
                }
                else if (card.UpgradeLevel == 2)
                {
                    // +50% tốc độ tấn công
                    spiritsParent.UpgradeSpiritStats(card.spiritType, 0.50f, 0, 0);
                }
                else if (card.UpgradeLevel == 3)
                {
                    // +50% tốc độ đạn
                    spiritsParent.UpgradeSpiritStats(card.spiritType, 0, 0.5f, 0);
                }
                else if (card.UpgradeLevel == 4)
                {
                    // +50% sát thương
                    spiritsParent.UpgradeSpiritStats(card.spiritType, 0, 0, 0.5f);
                }
                else if (card.UpgradeLevel == 5)
                {
                    // +100% → gấp đôi
                    spiritsParent.UpgradeSpiritStats(card.spiritType, 1f, 1f, 1f);
                }

                break;
            case SummonType.DragonMinion:
                // Triệu hồi minion
                DragonMinionParent dragonMinionParent = tower.GetComponentInChildren<DragonMinionParent>();
                if (dragonMinionParent == null)
                {
                    Debug.LogWarning("Tower không có component DragonMinionParent!");
                    return;
                }
                if (card.UpgradeLevel == 1)
                {
                    dragonMinionParent.CreateDragonMinion();
                }
                else if (card.UpgradeLevel == 2)
                {
                    dragonMinionParent.upgradeDragonMinionDamage(0.25f);
                }
                else if (card.UpgradeLevel == 3)
                {
                    dragonMinionParent.ExtendDragon(3);
                }
                else if (card.UpgradeLevel == 4)
                {
                    dragonMinionParent.upgradeDragonMinionSpeed(2f);
                }
                else if (card.UpgradeLevel == 5)
                {
                    dragonMinionParent.upgradeDragonMinionDamage(0.50f);
                    dragonMinionParent.upgradeDragonMinionSpeed(2f);
                }
                break;
            default:
                Debug.LogWarning("Loại triệu hồi không xác định!");
                break;
        }
    }
    private void SummonSpirits(UpgradeCard card, GameObject tower)
    {
        // Tìm MinionSlot trong Tower
        Transform spiritsSlot = tower.transform.Find("SpiritsSlot");
        if (spiritsSlot == null)
        {
            UnityEngine.Debug.LogWarning("MinionSlot không tìm thấy trong Tower!");
            return;
        }

        // Sinh ra prefab và đặt làm con của MinionSlot
        GameObject spirits = Instantiate(card.SpiritsPrefab, spiritsSlot.position, Quaternion.identity, spiritsSlot);
        Debug.Log("Minion được sinh ra tại: " + spirits.name);
    }
    public void Setup(UpgradeCard card, GameObject tower)
    {
        upgradeCard = card;
        Tower = tower;
    }
}
