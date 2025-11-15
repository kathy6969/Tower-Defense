using UnityEngine;
using UnityEngine.UI; // 👈 Đừng quên dòng này

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform centerPoint;

    [Header("Detection")]
    public LayerMask playerLayer;
    public Transform detectRange;      // vị trí trung tâm vùng phát hiện
    public Transform attackRange;      // vị trí trung tâm vùng tấn công
    public float detectRadius = 5f;    // 👈 chỉnh nhanh trong Inspector
    public float attackRadius = 2f;    // 👈 chỉnh nhanh trong Inspector

    [Header("States")]
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public BaseAttackState attackState;

    [Header("Stats")]
    public EnemyStatsData enemyData; // 👈 Kéo file ScriptableObject data vào đây
    private float currentHealth;
    private float currentDamage;
    private float maxHealth;
    private bool isDead = false;

    [Header("UI")]
    public Image healthBarFill; // 👈 Kéo fill-image của thanh máu vào đây

    [Header("Flip Logic")]
    [HideInInspector] public bool isFacingRight = true;
    // ----------------------------------

    [HideInInspector] public Transform targetPlayer;
    private EnemyState currentState;

    private float attackTimer;
    // -------------------------------------

    void Start()
    {
        InitializeStats();
        ChangeState(idleState);
    }

    void InitializeStats()
    {
        if (enemyData == null)
        {
            Debug.LogError("Chưa gán EnemyStatsData cho " + gameObject.name);
            return;
        }

        int levelFactor = Mathf.Max(0, enemyData.level - 1);

        maxHealth = enemyData.baseHealth + (enemyData.healthPerLevel * levelFactor);
        currentHealth = maxHealth;
        currentDamage = enemyData.baseDamage + (enemyData.damagePerLevel * levelFactor);

        isDead = false;

        // Cập nhật thanh máu lần đầu (để nó đầy)
        UpdateHealthBar();
    }

    void Update()
    {
        if (isDead) return;

        currentState?.OnUpdate();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (isDead && newState != null) return;

        if (currentState == newState) return;

        currentState?.OnExit();
        currentState = newState;
        currentState?.OnEnter(this);
    }

    // ======================
    // Các hàm gọi từ State
    // ======================

    public bool PlayerInDetectRange()
    {
        return Physics2D.OverlapCircle(detectRange.position, detectRadius, playerLayer);
    }

    public bool PlayerInAttackRange()
    {
        return Physics2D.OverlapCircle(attackRange.position, attackRadius, playerLayer);
    }

    public Transform GetPlayerTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(detectRange.position, detectRadius, playerLayer);
        if (hit != null) targetPlayer = hit.transform;
        return targetPlayer;
    }

    public bool IsAttackReady()
    {
        return attackTimer <= 0;
    }

    public void ResetAttackCooldown(float cooldown)
    {
        attackTimer = cooldown;
    }

    public float GetCurrentDamage()
    {
        return currentDamage;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " nhận " + damageAmount + " sát thương, còn " + currentHealth + " HP");

        // Cập nhật thanh máu UI (nếu có)
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // Tùy chọn: Bật animation "Bị đánh" (Hit)
            // animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " đã chết.");

        ChangeState(null);

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        Collider2D[] childColliders = GetComponentsInChildren<Collider2D>();
        foreach (var col in childColliders)
        {
            col.enabled = false;
        }

        animator.Play("Die"); // (Thay "Die" bằng tên animation chết của bạn)

        // Ẩn thanh máu khi chết
        if (healthBarFill != null)
        {
            // Tìm và tắt GameObject cha của thanh máu (thường là Canvas)
            healthBarFill.transform.parent.gameObject.SetActive(false);
        }

        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// Hàm cập nhật thanh máu
    /// </summary>
    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Tính toán tỉ lệ máu còn lại (từ 0 đến 1)
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }


    // ----- CÁC HÀM LẬT SPRITE (GIỮ NGUYÊN) -----
    public void CheckAndFlip(Vector2 direction)
    {
        if (isFacingRight && direction.x < 0)
        {
            Flip();
        }
        else if (!isFacingRight && direction.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
    // ------------------------------------------

    // ======================
    // Gizmos hiển thị vùng
    // ======================
    void OnDrawGizmosSelected()
    {
        if (detectRange != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectRange.position, detectRadius);
        }
        if (attackRange != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackRange.position, attackRadius);
        }
    }
}