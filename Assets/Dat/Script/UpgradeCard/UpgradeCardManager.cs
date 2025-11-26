using UnityEngine;
using System.Collections.Generic;

public class UpgradeCardManager : MonoBehaviour
{
    public UpgradeCardDatabase database;
    public List<string> ownedCardIDs = new List<string>();

    public void AddUpgrade(UpgradeCard card)
    {
        if (!ownedCardIDs.Contains(card.ID))
            ownedCardIDs.Add(card.ID);
    }

    // public void ApplyAllUpgrades(TowerShooter tower)
    // {
    //     foreach (var id in ownedCardIDs)
    //     {
    //         var card = database.GetByID(id);
    //         if (card != null)
    //             card.ApplyUpgrade(tower);
    //     }
    // }
    
    public bool HasUpgrade(UpgradeCard card)
    {
        return ownedCardIDs.Contains(card.ID);
    }


    // 🧠 Lưu tiến trình
    public void Save()
    {
        string data = JsonUtility.ToJson(new SaveData { ownedIDs = ownedCardIDs });
        PlayerPrefs.SetString("Upgrades", data);
    }

    // 🔄 Tải tiến trình
    public void Load()
    {
        if (PlayerPrefs.HasKey("Upgrades"))
        {
            string data = PlayerPrefs.GetString("Upgrades");
            SaveData save = JsonUtility.FromJson<SaveData>(data);
            ownedCardIDs = save.ownedIDs;
        }
    }
    public void ClearUpgrades()
    {
        ownedCardIDs.Clear();
    }

    [System.Serializable]
    private class SaveData
    {
        public List<string> ownedIDs;
    }
}
