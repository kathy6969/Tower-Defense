using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerEXP : MonoBehaviour
{
    [Header("Level Settings")]
    public int baseExpRequired = 100;   // EXP cần để lên từ Lv 0 → Lv 1
    public float expIncreaseFactor = 1.33f;  // 33% tăng theo mỗi cấp
    public int currentExp = 0;
    public int currentLevel = 0;
    public int expRequiredForNext;  // lưu giá trị exp cần cho cấp tiếp theo
    public Image expBarFill; // Tham chiếu đến Image fill của thanh EXP
    public TextMeshProUGUI levelText; // Tham chiếu đến Text hiển thị cấp độ
    public CardUpgradeUI cardUpgradeUI; // Tham chiếu đến CardUpgradeUI
    private void Start()
    {
        expRequiredForNext = GetRequiredExpForNextLevel();
        expBarFill.fillAmount = GetLevelProgress();
        levelText.text = $"Lv. {currentLevel}";
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        expBarFill.fillAmount = GetLevelProgress();
        levelText.text = $"Lv. {currentLevel}";
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // Nếu exp đủ lên cấp (và có thể vượt nhiều cấp)
        while (currentExp >= expRequiredForNext)
        {
            currentExp -= expRequiredForNext;
            currentLevel++;

            OnLevelUp();

            // Cập nhật mốc exp mới cho cấp kế
            expRequiredForNext = GetRequiredExpForNextLevel();
        }
    }

    private void OnLevelUp()
    {
        Debug.Log($"Tower leveled up! Current level: {currentLevel}");
        // Gợi ý: thêm hiệu ứng particle, tăng sát thương, tầm bắn...
        cardUpgradeUI.ShowCardUpgradePanel();
    }

    public int GetRequiredExpForNextLevel()
    {
        // EXP cần thiết tăng theo cấp
        return Mathf.RoundToInt(baseExpRequired * Mathf.Pow(expIncreaseFactor, currentLevel));
    }

    public float GetLevelProgress()
    {
        return (float)currentExp / expRequiredForNext;
    }

    public void ResetEXP()
    {
        currentExp = 0;
        currentLevel = 0;
        expRequiredForNext = GetRequiredExpForNextLevel();
    }
}
