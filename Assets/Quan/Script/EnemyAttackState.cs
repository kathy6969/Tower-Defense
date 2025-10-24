using UnityEngine;
using System.Collections;

// ⚠️ SỬA 1: Đổi lớp kế thừa
public class EnemyAttackState : BaseAttackState // 👈 Đổi từ EnemyState
{
    [Header("Cấu hình Cận chiến")]
    public float enableHitboxTime = 0.45f;
    public float disableHitboxTime = 0.54f;
    public GameObject attackBox;

    // ----- ĐÃ XÓA: attackCooldown, attackAnimationDuration, isAttacking -----
    // (Vì chúng đã nằm trong lớp cha 'BaseAttackState')

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy); // 👈 Gọi hàm của lớp cha
        if (attackBox != null)
            attackBox.SetActive(false);
    }

    // ----- ĐÃ XÓA: Hàm OnUpdate() -----
    // (Vì chúng ta dùng chung hàm OnUpdate() của lớp cha)

    // ⚠️ SỬA 2: Định nghĩa hàm AttackRoutine (bắt buộc)
    // Đây là phần logic riêng của Cận chiến
    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (animator != null && !string.IsNullOrEmpty(animationName))
            animator.Play(animationName);

        // Logic Hitbox
        yield return new WaitForSeconds(enableHitboxTime);
        if (attackBox != null)
            attackBox.SetActive(true);

        float hitboxDuration = disableHitboxTime - enableHitboxTime;
        if (hitboxDuration > 0)
            yield return new WaitForSeconds(hitboxDuration);
        if (attackBox != null)
            attackBox.SetActive(false);

        // Chờ hết anim
        float remainingAnimTime = attackAnimationDuration - disableHitboxTime;
        if (remainingAnimTime > 0)
            yield return new WaitForSeconds(remainingAnimTime);

        isAttacking = false;
        enemy.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit(); // 👈 Gọi hàm của lớp cha
        if (attackBox != null)
            attackBox.SetActive(false);
    }
}