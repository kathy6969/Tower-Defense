using UnityEngine;
using System.Collections.Generic;

public class TowerShooter : MonoBehaviour
{
    [Header("Attack Settings")]
    public float range = 5f;// Phạm vi tấn công
    public float fireRate = 1f;// Tốc độ bắn (số phát mỗi giây)
    public int maxTargets = 3;// Số lượng kẻ thù tối đa có thể tấn công cùng lúc
    public int damage = 10;// Sát thương mỗi viên đạn

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform MinionSlot; // Vị trí để triệu hồi tháp phụ

    private float fireCountdown = 0f;
    public List<Transform> currentTargets = new List<Transform>();
    void Start()
    {
        // Remove InvokeRepeating since we'll update before each shot
    }

    void UpdateTargets()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<(float distance, Transform enemy)> validEnemies = new List<(float, Transform)>();

        // Lọc những kẻ địch trong tầm
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= range)
                validEnemies.Add((distance, enemy.transform));
        }

        // Sắp xếp theo khoảng cách tăng dần
        validEnemies.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Giới hạn số lượng mục tiêu
        currentTargets.Clear();
        for (int i = 0; i < Mathf.Min(maxTargets, validEnemies.Count); i++)
        {
            currentTargets.Add(validEnemies[i].enemy);
        }
    }

    void Update()
    {
        if (fireCountdown <= 0f)
        {
            UpdateTargets(); // Update targets right before shooting
            if (currentTargets.Count > 0) // Only shoot if we have targets
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        foreach (Transform target in currentTargets)
        {
            if (target == null) continue;

            // Bắn viên đạn chính
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            TPDamage damage = bulletGO.GetComponent<TPDamage>();
            if (damage != null)
                damage.damageAmount = this.damage;
            if (bullet != null)
                bullet.Launch(target);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
