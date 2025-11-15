using UnityEngine;
using System.Collections;

public class Spitits : MonoBehaviour
{
    public enum SpiritType { Kim, Moc, Thuy, Hoa, Tho }
    public SpiritType spiritType;
    [Header("Attack Settings")]
    public TowerShooter towerShooter;
    public GameObject bulletPrefab;
    public float attackRate = 1f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;

    private float attackCountdown;
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;
    private SpiritsParent spiritsParent;

    void Start()
    {
        if (towerShooter == null)
            towerShooter = GetComponentInParent<TowerShooter>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
        spiritsParent = GetComponentInParent<SpiritsParent>();
        if (spiritsParent != null)
        {
            spiritsParent.RegisterChild(this.transform);
        }
    }

    void Update()
    {
        // Flip sprite theo hướng di chuyển
        if (spriteRenderer != null)
        {
            Vector3 moveDirection = transform.position - lastPosition;
            spriteRenderer.flipX = moveDirection.x < 0;
        }

        lastPosition = transform.position;

        // Tấn công
        attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0f)
        {
            Attack();
            attackCountdown = 1f / attackRate;
        }
    }

    void Attack()
    {
        var targets = towerShooter.currentTargets;
        if (targets == null || targets.Count == 0) return;

        foreach (Transform target in targets)
        {
            if (target == null) continue;

            GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Vector3 direction = (target.position - transform.position).normalized;
            Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.linearVelocity = direction * bulletSpeed;
            else
                StartCoroutine(MoveBullet(bulletGO, direction));

            Destroy(bulletGO, bulletLifetime);
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, Vector3 direction)
    {
        float elapsed = 0f;
        while (elapsed < bulletLifetime && bullet != null)
        {
            bullet.transform.position += direction * bulletSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
