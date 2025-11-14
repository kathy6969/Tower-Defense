using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeCardDatabase", menuName = "Tower/UpgradeCardDatabase")]
public class UpgradeCardDatabase : ScriptableObject
{
    public List<UpgradeCard> allCards;

    private Dictionary<string, UpgradeCard> _lookup;
    private bool isInitialized = false;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if (isInitialized) return;
        isInitialized = true;

        _lookup = new Dictionary<string, UpgradeCard>();

        foreach (var card in allCards)
        {
            if (card == null)
            {
                Debug.LogWarning("Có phần tử null trong UpgradeCardDatabase");
                continue;
            }

            if (string.IsNullOrEmpty(card.UpgradeID))
            {
                Debug.LogError($"Thẻ '{card.UpgradeName}' chưa có UpgradeID!");
                continue;
            }

            if (_lookup.ContainsKey(card.UpgradeID))
            {
                Debug.LogWarning($"Trùng ID: {card.UpgradeID} trong UpgradeCardDatabase");
                continue;
            }

            _lookup.Add(card.UpgradeID, card);
        }
    }

    public UpgradeCard GetByID(string id)
    {
        if (!isInitialized)
            Init();

        if (_lookup.TryGetValue(id, out var card))
            return card;

        Debug.LogWarning($"Không tìm thấy UpgradeCard có ID: {id}");
        return null;
    }
}
