using UnityEngine;

public class DragonHead : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 12f;
    public float turnSpeed = 260f;
    public float orbitDistance = 0.1f;

    [Header("History Settings")]
    public int historySize = 4000;
    public Vector2[] posHistory;
    public int historyIndex = 0;

    public float recordInterval = 0.015f;
    private float recordTimer;

    [Header("Refs")]
    public Rigidbody2D rb;
    private Transform target;
    private TowerShooter towerShooter;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        posHistory = new Vector2[historySize];

        towerShooter = GetComponentInParent<TowerShooter>();
    }

    void FixedUpdate()
    {
        UpdateTarget();
        MoveDragon();
        RecordHistory();
    }

    void UpdateTarget()
    {
        if (towerShooter == null || towerShooter.currentTargets.Count == 0)
        {
            target = null;
            return;
        }

        float best = float.MaxValue;
        Transform bestT = null;

        foreach (Transform t in towerShooter.currentTargets)
        {
            float d = Vector2.Distance(rb.position, t.position);
            if (d < best)
            {
                best = d;
                bestT = t;
            }
        }

        target = bestT;
    }

    void MoveDragon()
    {
        if (target == null)
        {
            rb.linearVelocity = transform.up * moveSpeed;
            return;
        }

        Vector2 toTarget = (Vector2)target.position - rb.position;
        float dist = toTarget.magnitude;

        Vector2 desiredDir = (dist > orbitDistance)
            ? toTarget.normalized
            : new Vector2(-toTarget.y, toTarget.x).normalized;

        float targetAngle = Mathf.Atan2(desiredDir.y, desiredDir.x) * Mathf.Rad2Deg - 90f;
        float currentAngle = rb.rotation;

        float diff = Mathf.DeltaAngle(currentAngle, targetAngle);
        float step = Mathf.Clamp(diff, -turnSpeed * Time.fixedDeltaTime, turnSpeed * Time.fixedDeltaTime);

        rb.MoveRotation(currentAngle + step);
        rb.linearVelocity = transform.up * moveSpeed;
    }

    void RecordHistory()
    {
        recordTimer += Time.fixedDeltaTime;
        if (recordTimer >= recordInterval)
        {
            recordTimer = 0;

            posHistory[historyIndex] = rb.position;
            historyIndex++;

            if (historyIndex >= historySize)
                historyIndex = 0;
        }
    }
}
