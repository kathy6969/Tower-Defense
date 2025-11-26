using UnityEngine;

public class DragonMinionParent : MonoBehaviour
{
    public DragonHead headPrefab;
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    public int bodyCount = 7;
    public float followDistance = 1f;

    public void CreateDragonMinion()
    {
        DragonHead head = Instantiate(headPrefab, transform.position, Quaternion.identity, transform);

        for (int i = 0; i < bodyCount; i++)
        {
            var body = Instantiate(bodyPrefab, head.transform.position, Quaternion.identity, transform);
            var seg = body.GetComponent<DragonSegment>();

            seg.head = head;
            seg.segmentIndex = i + 1;
            seg.followDistance = followDistance;
        }

        var tail = Instantiate(tailPrefab, head.transform.position, Quaternion.identity, transform);
        var tailSeg = tail.GetComponent<DragonSegment>();

        tailSeg.head = head;
        tailSeg.segmentIndex = bodyCount + 1;
        tailSeg.followDistance = followDistance;
    }
    public void upgradeDragonMinionSpeed(float speedIncrease)
    {
        DragonHead head = GetComponentInChildren<DragonHead>();
        if (head != null)
        {
            head.moveSpeed += speedIncrease;
            head.turnSpeed += 180f;
            //head.recordInterval *= 0.9f; 
        }
        var segments = GetComponentsInChildren<DragonSegment>();
        if (segments == null || segments.Length == 0) return;
        foreach (var seg in segments)
        {
            seg.followDistance *= 0.825f; // giảm khoảng cách theo dõi để tăng độ mượt
        }
    }
    public void upgradeDragonMinionDamage(float damageIncrease)
    {
        var hitboxes = GetComponentsInChildren<DragonHitbox>();
        if (hitboxes == null || hitboxes.Length == 0) return;

        foreach (var hb in hitboxes)
        {
            if (hb == null) continue;
            hb.damage = Mathf.RoundToInt(hb.damage * (1f + damageIncrease));
        }
    }
    public void ExtendDragon(int extraSegments)
    {
        if (extraSegments <= 0) return;

        DragonHead head = GetComponentInChildren<DragonHead>();
        if (head == null) return;

        // Tìm tail hiện tại
        DragonSegment tail = null;
        foreach (var seg in GetComponentsInChildren<DragonSegment>())
        {
            if (seg.segmentIndex > (tail?.segmentIndex ?? -1))
                tail = seg;
        }

        if (tail == null) return;

        int startIndex = tail.segmentIndex;  // điểm nối

        // Xóa tail cũ để thay thế bằng tail mới
        Vector3 tailPos = tail.transform.position;
        Quaternion tailRot = tail.transform.rotation;
        Destroy(tail.gameObject);

        // Sinh thêm body
        for (int i = 1; i <= extraSegments; i++)
        {
            GameObject body = Instantiate(bodyPrefab, tailPos, tailRot, transform);
            body.name = $"DragonBody_{startIndex + i - 1}";

            DragonSegment seg = body.GetComponent<DragonSegment>();
            seg.head = head;
            seg.segmentIndex = startIndex + i - 1;
            seg.followDistance = followDistance; // use existing property
            DragonHitbox hitbox = body.GetComponent<DragonHitbox>();
            if (hitbox != null)
            {
                hitbox.damage = Mathf.RoundToInt(hitbox.damage * 1.25f); // giữ nguyên sát thương
            }
        }

        // Sinh tail mới
        GameObject newTail = Instantiate(tailPrefab, tailPos, tailRot, transform);
        newTail.name = "DragonTail";
        DragonSegment tailSeg = newTail.GetComponent<DragonSegment>();
        tailSeg.head = head;
        tailSeg.segmentIndex = startIndex + extraSegments;
        tailSeg.followDistance = followDistance;
        DragonHitbox tailHitbox = newTail.GetComponent<DragonHitbox>();
        if (tailHitbox != null)
        {
            tailHitbox.damage = Mathf.RoundToInt(tailHitbox.damage * 1.25f); // giữ nguyên sát thương
        }
        // smoothingSpeed does not exist on DragonSegment; removed assignment
        Debug.Log("Rồng đã được kéo dài thêm!");
    }
}
