using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform centerPoint;

    [Header("Detection")]
    public LayerMask playerLayer;
    public Transform detectRange;
    public Transform attackRange;
    public float detectRadius = 5f;
    public float attackRadius = 2f;

    [Header("States")]
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public BaseAttackState attackState;   // Đúng yêu cầu

    [Header("Animator Control")]
    public float animatorSpeed = 1f; // 👈 BIẾN LOGIC TỐC ĐỘ MẶC ĐỊNH

    [Header("Flip Logic")]
    [HideInInspector] public bool isFacingRight = true;

    [HideInInspector] public Transform targetPlayer;
    private EnemyState currentState;

    private float attackTimer;

    void Start()
    {
        // 👇 SỬA ĐỔI QUAN TRỌNG: XÓA việc chỉnh tốc độ Animator cố định khi bắt đầu.
        // Tốc độ mặc định sẽ được quản lý bởi EnemyState.OnEnter()
        // if (animator != null)
        // {
        //     animator.speed = animatorSpeed;
        // }

        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdate();

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
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

    public bool IsAttackReady()
    {
        return attackTimer <= 0;
    }

    public void ResetAttackCooldown(float cooldown)
    {
        attackTimer = cooldown;
    }

    // ======================
    // ANIMATOR SPEED CONTROL
    // ======================
    public void SetAnimatorSpeed(float speed)
    {
        if (animator != null)
        {
            animator.speed = speed;
        }
    }

    // ----- Lật Sprite -----
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

    // ----- Gizmos -----
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