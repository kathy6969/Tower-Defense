using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UpgradeCardDatabase", menuName = "Tower/UpgradeCardDatabase")]
public class UpgradeCardDatabase : ScriptableObject
{
    public List<UpgradeCard> allCards;

    private Dictionary<string, UpgradeCard> _lookup;

    public void Init()
    {
        _lookup = new Dictionary<string, UpgradeCard>();
        foreach (var card in allCards)
        {
            if (!_lookup.ContainsKey(card.UpgradeID))
                _lookup.Add(card.UpgradeID, card);
            else
                Debug.LogWarning($"Trùng ID: {card.UpgradeID} trong UpgradeCardDatabase");
        }
    }

    public UpgradeCard GetByID(string id)
    {
        if (_lookup == null || _lookup.Count == 0)
            Init();

        if (_lookup.TryGetValue(id, out var card))
            return card;
        return null;
    }
}
