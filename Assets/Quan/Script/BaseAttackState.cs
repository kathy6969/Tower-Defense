using UnityEngine;
using System.Collections;

// Đây là lớp cha chung cho mọi loại tấn công
// Nó chứa logic OnUpdate chung
public abstract class BaseAttackState : EnemyState
{
    [Header("Cấu hình Tấn công chung")]
    public float attackCooldown = 1.5f;     // Biến chung
    [Tooltip("Tổng thời gian của animation tấn công")]
    public float attackAnimationDuration = 1.0f; // Biến chung

    protected bool isAttacking; // Biến chung

    // 'abstract' nghĩa là: Bất cứ 'con' nào kế thừa
    // BẮT BUỘC phải tự viết hàm AttackRoutine()
    protected abstract IEnumerator AttackRoutine();

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy);
        isAttacking = false;
    }

    // Đây là logic OnUpdate CHUNG cho cả 2 kiểu tấn công
    public override void OnUpdate()
    {
        // 1. Ưu tiên 1: Nếu player rời khỏi vùng tấn công
        if (!enemy.PlayerInAttackRange())
        {
            enemy.ChangeState(enemy.moveState);
            return;
        }

        // 2. Nếu đang trong animation
        if (isAttacking)
        {
            return;
        }

        // 3. Nếu HẾT hồi chiêu (dùng hàm của Controller)
        if (enemy.IsAttackReady())
        {
            // Báo cho Controller biết cooldown của MÌNH là bao nhiêu
            enemy.ResetAttackCooldown(this.attackCooldown);
            enemy.StartCoroutine(AttackRoutine());
        }

        // 4. Nếu còn cooldown -> Đứng im (IdleState sẽ lo việc kéo lại)
    }

    public override void OnExit()
    {
        base.OnExit();
        isAttacking = false;
    }
}