using UnityEngine;
using System.Collections.Generic;

public class TowerShooter : MonoBehaviour
{
    [Header("Attack Settings")]
    public float range = 5f;// Phạm vi tấn công
    public float fireRate = 1f;// Tốc độ bắn (số phát mỗi giây)
    public int maxTargets = 3;// Số lượng kẻ thù tối đa có thể tấn công cùng lúc

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private List<Transform> currentTargets = new List<Transform>();

    void Start()
    {
        InvokeRepeating(nameof(UpdateTargets), 0f, 0.5f);
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
        if (currentTargets.Count == 0)
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
        foreach (Transform target in currentTargets)
        {
            if (target == null) continue;

            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Launch(target);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
