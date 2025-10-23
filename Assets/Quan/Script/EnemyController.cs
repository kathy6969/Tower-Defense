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
    public EnemyAttackState attackState;

    [HideInInspector] public Transform targetPlayer;
    private EnemyState currentState;

    void Start()
    {
        ChangeState(idleState);
    }

    void Update()
    {
        currentState?.OnUpdate();
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
