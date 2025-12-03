using UnityEngine;
using System.Collections;

public class EnemyAttackState : BaseAttackState
{
    [Header("Cấu hình Cận chiến")]
    public GameObject attackBox;

    // 👉 THÊM: Tốc độ Animator cho riêng trạng thái tấn công
    [Header("Attack Animation Speed")]
    public float attackAnimSpeed = 1.5f; // Đặt tốc độ tấn công nhanh hơn mặc định

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy);

        // 👉 THÊM: Thiết lập tốc độ tấn công (chỉ ảnh hưởng đến animation tấn công)
        enemy.SetAnimatorSpeed(attackAnimSpeed);

        // Bật hitbox ngay khi vào state
        if (attackBox != null)
            attackBox.SetActive(true);
    }

    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // SỬA QUAN TRỌNG: dùng enemy.animator thay vì animator trong State
        // **LƯU Ý:** Tốc độ đã được đặt trong OnEnter
        if (enemy.animator != null && !string.IsNullOrEmpty(animationName))
            enemy.animator.Play(animationName);

        // Hitbox luôn bật trong toàn bộ animation
        yield return new WaitForSeconds(attackAnimationDuration);

        isAttacking = false;

        // Quay lại Idle
        enemy.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit();

        // 👉 THÊM: Trả lại tốc độ Animator về tốc độ MẶC ĐỊNH của Controller 
        // (để đảm bảo các State tiếp theo dùng tốc độ bình thường)
        enemy.SetAnimatorSpeed(enemy.animatorSpeed);

        // Tắt hitbox khi rời khỏi state
        if (attackBox != null)
            attackBox.SetActive(false);
    }
}