using UnityEngine;

public class CardSelec : MonoBehaviour
{
    public CardUpgradeUI cardUpgradeUI;
    public UpgradeApplier upgradeApplier;
    [HideInInspector] public UpgradeCard upgradeCard;
    [HideInInspector] public GameObject Tower;
    [HideInInspector] public UpgradeCardManager upgradeCardManager;
    
    void Start()
    {
        cardUpgradeUI = GetComponentInParent<CardUpgradeUI>();
        upgradeCardManager = GetComponentInParent<UpgradeCardManager>();
        upgradeApplier = GetComponentInParent<UpgradeApplier>();
    }
    
    public void OnClick()
    {
        // Kiểm tra các tham số chính
        if (upgradeCard == null)
        {
            Debug.LogWarning("UpgradeCard chưa được setup!");
            return;
        }
        
        if (Tower == null)
        {
            Debug.LogWarning("Tower chưa được setup!");
            return;
        }
        
        // Kiểm tra từng component
        if (cardUpgradeUI == null)
        {
            cardUpgradeUI = GetComponentInParent<CardUpgradeUI>();
            if (cardUpgradeUI == null)
            {
                Debug.LogWarning("cardUpgradeUI không tìm thấy!");
                return;
            }
        }
        
        if (upgradeCardManager == null)
        {
            upgradeCardManager = GetComponentInParent<UpgradeCardManager>();
            if (upgradeCardManager == null)
            {
                Debug.LogWarning("upgradeCardManager không tìm thấy!");
                return;
            }
        }
        
        if (upgradeApplier == null)
        {
            upgradeApplier = GetComponentInParent<UpgradeApplier>();
            if (upgradeApplier == null)
            {
                Debug.LogWarning("upgradeApplier không tìm thấy!");
                return;
            }
        }
        
        // Áp dụng nâng cấp
        upgradeApplier.Setup(upgradeCard, Tower);
        upgradeApplier.ApplyUpgrade(upgradeCard, Tower);
        upgradeCardManager.AddUpgrade(upgradeCard);
        cardUpgradeUI.HideCardUpgradePanel();
    }
    
    public void Setup(UpgradeCard card, GameObject tower)
    {
        upgradeCard = card;
        Tower = tower;
    }
}
