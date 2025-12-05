using UnityEngine;
public class UpgradeApplier : MonoBehaviour
{
    [HideInInspector] public UpgradeCard upgradeCard;
    [HideInInspector] public GameObject Tower;
    [HideInInspector] public TowerShooter towerShooter;
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
            case UpgradeCardType.ExtraWeapon:
                ApplyExtraWeaponUpgrade(upgradeCard, Tower);
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
                    Debug.LogWarning("Tower không có component SpiritsParent!");
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
            case SummonType.ElectricOrb:
                if (card.UpgradeLevel == 1)
                {
                    SummonElectricOrb(card, tower);
                }
                else if (card.UpgradeLevel == 2)
                {
                    ElectricOrb electricOrb = tower.GetComponentInChildren<ElectricOrb>();
                    if (electricOrb != null)
                    {
                        electricOrb.IncreaseOrbCount(2);
                    }
                }
                else if (card.UpgradeLevel == 3)
                {
                    ElectricOrb electricOrb = tower.GetComponentInChildren<ElectricOrb>();
                    if (electricOrb != null)
                    {
                        electricOrb.IncreaseRotationSpeed(20f);
                    }
                }
                else if (card.UpgradeLevel == 4)
                {
                    ElectricOrb electricOrb = tower.GetComponentInChildren<ElectricOrb>();
                    if (electricOrb != null)
                    {
                        electricOrb.IncreaseOrbCount(3);
                    }
                }
                else if (card.UpgradeLevel == 5)
                {
                    ElectricOrb electricOrb = tower.GetComponentInChildren<ElectricOrb>();
                    if (electricOrb != null)
                    {
                        electricOrb.SpawnReverseOrbs();
                    }
                }
                break;
            default:
                Debug.LogWarning("Loại triệu hồi không xác định!");
                break;
        }
    }
    private void ApplyExtraWeaponUpgrade(UpgradeCard card, GameObject tower)
    {
        switch (card.extraWeaponType)
        {
            case ExtraWeaponType.MinecraftTNT:
                // Thêm vũ khí phụ Minecraft TNT
                if (upgradeCard.UpgradeLevel == 1)
                {
                    SummonMinecraftTNT(card, tower);
                }
                else if (upgradeCard.UpgradeLevel == 2)
                {
                    // Giảm thời gian coldDown ném TNT
                    MinecraftTNT minecraftTNT = tower.GetComponentInChildren<MinecraftTNT>();
                    if (minecraftTNT != null)
                    {
                        minecraftTNT.throwCooldown *= 0.9f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 3)
                {
                    // Tăng số lượng TNT mỗi lần ném
                    MinecraftTNT minecraftTNT = tower.GetComponentInChildren<MinecraftTNT>();
                    if (minecraftTNT != null)
                    {
                        minecraftTNT.tntCountPerThrow += 1;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 4)
                {
                    // giảm thời gian coldDown ném TNT
                    MinecraftTNT minecraftTNT = tower.GetComponentInChildren<MinecraftTNT>();
                    if (minecraftTNT != null)
                    {
                        minecraftTNT.throwCooldown *= 0.8f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 5)
                {
                    // tăng số lượng TNT mỗi lần ném
                    MinecraftTNT minecraftTNT = tower.GetComponentInChildren<MinecraftTNT>();
                    if (minecraftTNT != null)
                    {
                        minecraftTNT.tntCountPerThrow += 2;
                    }
                }
                break;
            case ExtraWeaponType.ThrowingAxes:
                if (upgradeCard.UpgradeLevel == 1)
                {
                    SummonThrowingAxes(card, tower);
                }
                else if (upgradeCard.UpgradeLevel == 2)
                {
                    StealAxes stealAxes = tower.GetComponentInChildren<StealAxes>();
                    if (stealAxes != null)
                    {
                        stealAxes.throwCooldown *= 0.9f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 3)
                {
                    StealAxes stealAxes = tower.GetComponentInChildren<StealAxes>();
                    if (stealAxes != null)
                    {
                        stealAxes.throwCooldown *= 0.85f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 4)
                {
                    StealAxes stealAxes = tower.GetComponentInChildren<StealAxes>();
                    if (stealAxes != null)
                    {
                        stealAxes.throwCooldown *= 0.8f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 5)
                {
                    StealAxes stealAxes = tower.GetComponentInChildren<StealAxes>();
                    if (stealAxes != null)
                    {
                        stealAxes.throwCooldown *= 0.75f;
                    }
                }
                break;
            case ExtraWeaponType.ThrowingSickle:
                if (upgradeCard.UpgradeLevel == 1)
                {
                    SummonThrowingSickle(card, tower);
                }
                else if (upgradeCard.UpgradeLevel == 2)
                {
                    StealAxes throwingSickle = tower.GetComponentInChildren<StealAxes>();
                    if (throwingSickle != null)
                    {
                        throwingSickle.throwCooldown *= 0.9f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 3)
                {
                    StealAxes throwingSickle = tower.GetComponentInChildren<StealAxes>();
                    if (throwingSickle != null)
                    {
                        throwingSickle.throwCooldown *= 0.85f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 4)
                {
                    StealAxes throwingSickle = tower.GetComponentInChildren<StealAxes>();
                    if (throwingSickle != null)
                    {
                        throwingSickle.throwCooldown *= 0.8f;
                    }
                }
                else if (upgradeCard.UpgradeLevel == 5)
                {
                    StealAxes throwingSickle = tower.GetComponentInChildren<StealAxes>();
                    if (throwingSickle != null)
                    {
                        throwingSickle.throwCooldown *= 0.75f;
                    }
                }
                break;
            default:
                Debug.LogWarning("Loại vũ khí phụ không xác định!");
                break;
        }
    }
    private void SummonSpirits(UpgradeCard card, GameObject tower)
    {
        // Tìm MinionSlot trong Tower
        Transform spiritsSlot = tower.transform.Find("SpiritsSlot");
        if (spiritsSlot == null)
        {
            Debug.LogWarning("MinionSlot không tìm thấy trong Tower!");
            return;
        }
        // Sinh ra prefab và đặt làm con của MinionSlot
        GameObject spirits = Instantiate(card.SummonPrefab, spiritsSlot.position, Quaternion.identity, spiritsSlot);
        Debug.Log("Minion được sinh ra tại: " + spirits.name);
    }
    private void SummonElectricOrb(UpgradeCard card, GameObject tower)
    {
        GameObject electricOrb = Instantiate(card.SummonPrefab, tower.transform.position, Quaternion.identity, tower.transform);
        electricOrb.transform.parent = tower.transform;
        Debug.Log("Electric Orb được sinh ra tại: " + electricOrb.name);
    }
    private void SummonMinecraftTNT(UpgradeCard card, GameObject tower)
    {
        GameObject minecraftTNT = Instantiate(card.ExtraWeaponPrefab, tower.transform.position, Quaternion.identity, tower.transform);
        minecraftTNT.transform.parent = tower.transform;
        Debug.Log("Minecraft TNT được sinh ra tại: " + minecraftTNT.name);
    }
    private void SummonThrowingAxes(UpgradeCard card, GameObject tower)
    {
        GameObject throwingAxes = Instantiate(card.ExtraWeaponPrefab, tower.transform.position, Quaternion.identity, tower.transform);
        throwingAxes.transform.parent = tower.transform;
        Debug.Log("Throwing Axes được sinh ra tại: " + throwingAxes.name);
    }
    private void SummonThrowingSickle(UpgradeCard card, GameObject tower)
    {
        GameObject throwingSickle = Instantiate(card.ExtraWeaponPrefab, tower.transform.position, Quaternion.identity, tower.transform);
        throwingSickle.transform.parent = tower.transform;
        Debug.Log("Throwing Sickle được sinh ra tại: " + throwingSickle.name);
    }
    public void Setup(UpgradeCard card, GameObject tower)
    {
        upgradeCard = card;
        Tower = tower;
    }
}
