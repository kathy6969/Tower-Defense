using UnityEngine;

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

    // ⚠️ SỬA 1: ĐÃ ĐỔI KIỂU CỦA 'attackState'
    // Giờ nó có thể nhận BẤT KỲ state nào kế thừa từ 'BaseAttackState'
    // (Bạn kéo EnemyAttackState hay EnemyRangedAttackState vào đây đều được)
    public BaseAttackState attackState;

    [Header("Flip Logic")]
    [HideInInspector] public bool isFacingRight = true;
    // ----------------------------------

    [HideInInspector] public Transform targetPlayer;
    private EnemyState currentState;

    // ----- 1. BIẾN ĐỂ ĐẾM COOLDOWN -----
    private float attackTimer;
    // -------------------------------------

    void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdate();

        // ----- 2. LOGIC ĐẾM NGƯỢC COOLDOWN (LUÔN CHẠY) -----
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        // --------------------------------------------------
    }

    public void ChangeState(EnemyState newState)
    {
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

    // ----- 3. HÀM ĐỂ STATE KIỂM TRA COOLDOWN -----
    public bool IsAttackReady()
    {
        return attackTimer <= 0;
    }

    // ----- 4. HÀM ĐỂ ATTACKSTATE RESET COOLDOWN (ĐÃ SỬA) -----
    // ⚠️ SỬA 2: Sửa hàm này để nó 'nhận' cooldown
    public void ResetAttackCooldown(float cooldown)
    {
        // Nó nhận giá trị cooldown từ bất kỳ state nào gọi nó
        attackTimer = cooldown;
    }
    // -------------------------------------------------


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