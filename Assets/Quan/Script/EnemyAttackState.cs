using UnityEngine;
using System.Collections;

public class EnemyAttackState : EnemyState
{
    [Header("Cấu hình thời gian")]
    public float attackCooldown = 1.5f;     // Thời gian hồi chiêu (chờ giữa các đòn)
    [Tooltip("Tổng thời gian của animation tấn công")]
    public float attackAnimationDuration = 1.0f; // 👈 BIẾN MỚI
    public float enableHitboxTime = 0.45f;  // Thời điểm bật hitbox
    public float disableHitboxTime = 0.54f; // Thời điểm tắt hitbox

    private float attackTimer;
    private bool isAttacking;

    [Header("Hitbox tấn công")]
    public GameObject attackBox;

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy);

        if (attackTimer <= 0)
            attackTimer = 0f;

        isAttacking = false;

        if (attackBox != null)
            attackBox.SetActive(false);
    }

    // HÀM MỚI: Để các state khác kiểm tra
    public bool IsAttackReady()
    {
        // Sẵn sàng khi hết hồi chiêu VÀ không đang trong một đòn đánh dở dang
        return attackTimer <= 0 && !isAttacking;
    }

    public override void OnUpdate()
    {
        attackTimer -= Time.deltaTime;

        // 1. Ưu tiên 1: Nếu player rời khỏi vùng tấn công → quay lại Đuổi theo
        if (!enemy.PlayerInAttackRange())
        {
            enemy.ChangeState(enemy.EnemyMoveState); // 👈 Đã sửa tên
            return;
        }

        // 2. Nếu đang trong animation tấn công (isAttacking == true)
        if (isAttacking)
        {
            return;
        }

        // 3. Nếu HẾT hồi chiêu (và không đang tấn công)
        if (IsAttackReady())
        {
            attackTimer = attackCooldown;
            enemy.StartCoroutine(AttackRoutine());
        }

        // 4. Nếu CÒN hồi chiêu -> CỨ ĐỨNG YÊN.
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // Gọi anim attack
        if (animator != null && !string.IsNullOrEmpty(animationName))
            animator.Play(animationName);

        // Bật hitbox
        yield return new WaitForSeconds(enableHitboxTime);
        if (attackBox != null)
            attackBox.SetActive(true);

        // Tắt hitbox
        float hitboxDuration = disableHitboxTime - enableHitboxTime;
        if (hitboxDuration > 0)
            yield return new WaitForSeconds(hitboxDuration);

        if (attackBox != null)
            attackBox.SetActive(false);

        // Chờ cho phần còn lại của animation chạy xong
        float remainingAnimTime = attackAnimationDuration - disableHitboxTime;
        if (remainingAnimTime > 0)
            yield return new WaitForSeconds(remainingAnimTime);

        isAttacking = false;

        // Tấn công xong, chuyển về IDLE để chờ cooldown
        enemy.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit();
        if (attackBox != null)
            attackBox.SetActive(false);
        isAttacking = false;
    }
}