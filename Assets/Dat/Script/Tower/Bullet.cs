using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Flight Settings")]
    public float speed = 10f;              // Tốc độ di chuyển
    public AnimationCurve heightCurve;     // Độ cong đường bay
    public float maxHeight = 1.5f;         // Độ cao tối đa

    private Transform target;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float travelPercent;           // Tiến độ bay (0 → 1)
    private float totalDistance;

    public void Launch(Transform _target)
    {
        target = _target;
        startPos = transform.position;
        targetPos = _target.position;
        totalDistance = Vector3.Distance(startPos, targetPos);
        travelPercent = 0f;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Tăng tiến độ di chuyển (0 → 1)
        travelPercent += (speed / totalDistance) * Time.deltaTime;
        travelPercent = Mathf.Clamp01(travelPercent);

        // Tính vị trí giữa hai điểm
        Vector3 flatPos = Vector3.Lerp(startPos, targetPos, travelPercent);

        // Tính độ cao cong theo curve
        float height = heightCurve.Evaluate(travelPercent) * maxHeight;

        // Vị trí hiện tại
        Vector3 currentPos = new Vector3(flatPos.x, flatPos.y + height, flatPos.z);
        transform.position = currentPos;

        // Tính hướng di chuyển
        Vector3 nextPos = Vector3.Lerp(startPos, targetPos, Mathf.Clamp01(travelPercent + 0.01f));
        nextPos.y += heightCurve.Evaluate(Mathf.Clamp01(travelPercent + 0.01f)) * maxHeight;
        Vector2 direction = (nextPos - currentPos).normalized;
        
        // Tính góc xoay (không trừ 90 độ nữa)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Đến mục tiêu thì phá hủy
        if (travelPercent >= 1f)
            HitTarget();
    }

    void HitTarget()
    {
        //Debug.Log("Bullet hit the target!");
        // Có thể thêm hiệu ứng va chạm ở đây
        //Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            HitTarget();
    }
}
