using UnityEngine;

public class DragonSegment : MonoBehaviour
{
    public DragonHead head;
    public int segmentIndex;

    public float followDistance = 1f;
    public float waveStrength = 0.35f;
    public float waveSpeed = 4f;

    void LateUpdate()
    {
        if (head == null) return;

        // Tổng distance = số thân * followDistance
        float dist = segmentIndex * followDistance;

        // index thực trong history
        int baseIndex = head.historyIndex - Mathf.RoundToInt(dist / head.recordInterval);

        if (baseIndex < 0)
            baseIndex += head.historySize;

        int nextIndex = (baseIndex - 1 + head.historySize) % head.historySize;

        Vector2 posA = head.posHistory[baseIndex];
        Vector2 posB = head.posHistory[nextIndex];

        // Nội suy → mượt tuyệt đối
        Vector2 dir = posB - posA;
        Vector2 finalPos = posA + dir * 0.35f;

        // Wave motion nhẹ giống Terraria
        float wave = Mathf.Sin((Time.time * waveSpeed) + segmentIndex * 0.4f) * waveStrength;
        Vector2 waveOffset = new Vector2(-dir.y, dir.x).normalized * wave;

        transform.position = finalPos + waveOffset;

        if (dir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
