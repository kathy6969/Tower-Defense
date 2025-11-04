using UnityEngine;
using System.Collections.Generic;

public class ChildActivator : MonoBehaviour
{
    [Header("Child Settings")]
    public bool randomizeOnStart = true;  // Có tự động disable khi khởi động không
    private List<GameObject> children = new List<GameObject>();

    public TowerEXP towerExp; // Tham chiếu đến TowerEXP

    void Awake()
    {
        // Lấy tất cả object con trực tiếp
        children.Clear();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        if (randomizeOnStart)
            DisableAllExceptOne();
    }

    /// <summary>
    /// Tắt tất cả object con, chỉ bật lại một object ngẫu nhiên.
    /// </summary>
    public void DisableAllExceptOne()
    {
        if (children.Count == 0) return;

        // Chọn ngẫu nhiên một object để giữ lại
        int indexToKeep = Random.Range(0, children.Count);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetActive(i == indexToKeep);
        }
    }

    /// <summary>
    /// Bật ngẫu nhiên một số lượng object con trong khoảng cho trước.
    /// </summary>
    public void EnableRandomChildren(int minCount, int maxCount)
    {
        if (children.Count == 0) return;

        // Disable toàn bộ trước
        foreach (var c in children)
            c.SetActive(false);

        // Lấy số lượng cần bật (giới hạn trong khoảng hợp lệ)
        int countToEnable = Mathf.Clamp(Random.Range(minCount, maxCount + 1), 1, children.Count);

        // Tạo danh sách index ngẫu nhiên
        List<int> indices = new List<int>();
        for (int i = 0; i < children.Count; i++) indices.Add(i);
        Shuffle(indices);

        // Bật theo số lượng được chọn
        for (int i = 0; i < countToEnable; i++)
        {
            int idx = indices[i];
            children[idx].SetActive(true);
        }
        towerExp.AddExp(Random.Range(25, 100)); // Thêm EXP ngẫu nhiên từ 25 đến 100 mỗi khi kích hoạt
    }

    // Hàm trộn ngẫu nhiên danh sách index
    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
