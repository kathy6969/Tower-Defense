using UnityEngine;

public class TowerShooter : MonoBehaviour
{
    [SerializeField] private float range = 5f; // Phạm vi tấn công
    [SerializeField] private float fireRate = 1f; // Tốc độ bắn (giây)
    [SerializeField] private GameObject bulletPrefab; // Prefab đạn
    [SerializeField] private Transform firePoint; // Điểm bắn đạn

    private float fireCountdown = 0f;
    private Transform target;
    
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); // Cập nhật target mỗi 0.5s
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi tấn công trong Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
