using UnityEngine;

public class CardSelec : MonoBehaviour
{
    public CardUpgradeUI cardUpgradeUI;
    [HideInInspector] public UpgradeCard upgradeCard;
    [HideInInspector] public TowerShooter towerShooter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardUpgradeUI = GetComponentInParent<CardUpgradeUI>();
        towerShooter = FindAnyObjectByType<TowerShooter>();
    }
    public void OnClick()
    {
        if (upgradeCard == null || towerShooter == null)
        {
            Debug.LogWarning("UpgradeCardButton chưa được setup đúng!");
            return;
        }
        // Gọi upgrade
        upgradeCard.ApplyUpgrade(towerShooter);
        // Ẩn UI nâng cấp thẻ
        cardUpgradeUI.HideCardUpgradePanel();
    }
    // Hàm setup khi spawn thẻ
    public void Setup(UpgradeCard card, TowerShooter tower)
    {
        upgradeCard = card;
        towerShooter = tower;
    }
}
