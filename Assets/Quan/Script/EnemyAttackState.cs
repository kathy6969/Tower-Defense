using UnityEngine;
using System.Collections;

public class EnemyAttackState : EnemyState
{
    [Header("Cấu hình tấn công")]
    public float attackCooldown = 1.5f;     // Thời gian hồi chiêu
    public float enableHitboxTime = 0.45f;  // Thời điểm bật hitbox
    public float disableHitboxTime = 0.54f; // Thời điểm tắt hitbox

    private float attackTimer;
    private bool isAttacking;

    [Header("Hitbox tấn công")]
    public GameObject attackBox;

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy);

        // Reset timer để bắt đầu tính hồi chiêu
        if (attackTimer <= 0)
            attackTimer = 0f;

        isAttacking = false;

        // Tắt hitbox an toàn
        if (attackBox != null)
            attackBox.SetActive(false);
    }

    public override void OnUpdate()
    {
        attackTimer -= Time.deltaTime;

        // Nếu player rời khỏi vùng tấn công → quay lại Move
        if (!enemy.PlayerInAttackRange())
        {
            enemy.ChangeState(enemy.moveState);
            return;
        }

        // Nếu đang hồi chiêu mà chưa sẵn sàng tấn công → đứng idle
        if (attackTimer > 0 && !isAttacking)
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        // Nếu hết hồi chiêu và chưa tấn công
        if (attackTimer <= 0 && !isAttacking)
        {
            attackTimer = attackCooldown;
            enemy.StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Gọi anim attack
        if (animator != null && !string.IsNullOrEmpty(animationName))
            animator.Play(animationName);

        // Bật hitbox tại thời điểm xác định
        yield return new WaitForSeconds(enableHitboxTime);
        if (attackBox != null)
            attackBox.SetActive(true);

        // Tắt hitbox
        yield return new WaitForSeconds(disableHitboxTime - enableHitboxTime);
        if (attackBox != null)
            attackBox.SetActive(false);

        // Thêm thời gian đợi anim kết thúc
        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
    }

    public override void OnExit()
    {
        base.OnExit();

        // Khi thoát state, đảm bảo hitbox tắt
        if (attackBox != null)
            attackBox.SetActive(false);

        isAttacking = false;
    }
}
