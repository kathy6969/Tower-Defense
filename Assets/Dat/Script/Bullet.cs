using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    [SerializeField] private float speed = 70f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Tính hướng di chuyển
        Vector3 dir = target.position - transform.position;
        
        // Tính góc xoay dựa trên hướng di chuyển
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; // Trừ 90 độ nếu sprite của đạn mặc định hướng lên
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Di chuyển đạn
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(Vector2.up * distanceThisFrame, Space.Self); // Sử dụng Vector2.up vì đạn đã được xoay
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
