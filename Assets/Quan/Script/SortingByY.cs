using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingByY : MonoBehaviour
{
    [Header("Sorting Control")]
    public bool applyToAll = true;         // Nếu false thì chỉ áp dụng khi trùng layer
    public string targetSortingLayer = "Character"; // Layer đích
    public int minOrderInLayer = 0;        // Hoặc chỉ áp dụng cho sprite có order >= giá trị này

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (!applyToAll)
        {
            // Kiểm tra sorting layer hoặc order trước khi áp dụng
            if (spriteRenderer.sortingLayerName != targetSortingLayer &&
                spriteRenderer.sortingOrder < minOrderInLayer)
                return;
        }

        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
