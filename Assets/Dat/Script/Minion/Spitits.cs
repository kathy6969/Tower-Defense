using UnityEngine;
using System.Collections;

public class Spitits : MonoBehaviour
{
    [Header("Movement Settings")]
    public float rotationSpeed = 100f;    
    public float orbitRadius = 2f;        
    public float attackRate = 1f;         

    [Header("References")]
    public TowerShooter towerShooter;     
    public GameObject BulletPrefab;       

    [Header("Bullet Settings")]
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;

    private float attackCountdown = 0f;
    private Vector3 centerPosition;
    private float currentAngle = 0f;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    void Start()
    {
        if (towerShooter == null)
        {
            towerShooter = GetComponentInParent<TowerShooter>();
        }
        centerPosition = towerShooter.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void Update()
    {
        // Update orbit position (2D)
        currentAngle += rotationSpeed * Time.deltaTime;
        float x = centerPosition.x + orbitRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = centerPosition.y + orbitRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        transform.position = new Vector3(x, y, 0f); // Keep Z at 0 for 2D

        // Handle sprite flipping
        if (spriteRenderer != null)
        {
            Vector3 moveDirection = transform.position - lastPosition;
            spriteRenderer.flipX = moveDirection.x < 0;
        }

        lastPosition = transform.position;

        // Handle attacking
        if (attackCountdown <= 0f)
        {
            Attack();
            attackCountdown = 1f / attackRate;
        }

        attackCountdown -= Time.deltaTime;
    }

    void Attack()
    {
        var targets = towerShooter.currentTargets;
        if (targets == null || targets.Count == 0) return;

        foreach (Transform target in targets)
        {
            if (target == null) continue;

            GameObject bulletGO = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            
            // Calculate direction to target
            Vector3 direction = (target.position - transform.position).normalized;
            
            // Add movement using Rigidbody2D if it exists
            Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
            else
            {
                // If no Rigidbody2D, start coroutine to move the bullet
                StartCoroutine(MoveBullet(bulletGO, direction));
            }

            // Destroy bullet after lifetime
            Destroy(bulletGO, bulletLifetime);
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, Vector3 direction)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = bullet.transform.position;

        while (elapsedTime < bulletLifetime && bullet != null)
        {
            bullet.transform.position += direction * bulletSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        if (towerShooter != null)
        {
            // Draw orbit path for 2D
            Gizmos.color = Color.yellow;
            Vector3 center = towerShooter != null ? towerShooter.transform.position : Vector3.zero;
            Gizmos.DrawWireSphere(center, orbitRadius);
        }
    }
}
