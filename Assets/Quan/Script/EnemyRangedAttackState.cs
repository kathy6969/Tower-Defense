using UnityEngine;
using System.Collections;

// ⚠️ SỬA 1: Đổi lớp kế thừa
public class EnemyRangedAttackState : BaseAttackState // 👈 Đổi từ EnemyState
{
    [Header("Cấu hình Tầm xa")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    // ----- ĐÃ XÓA: attackCooldown, attackAnimationDuration, isAttacking -----
    // (Vì chúng đã nằm trong lớp cha 'BaseAttackState')

    // ----- ĐÃ XÓA: Hàm OnUpdate() -----
    // (Vì chúng ta dùng chung hàm OnUpdate() của lớp cha)

    // ⚠️ SỬA 2: Định nghĩa hàm AttackRoutine (bắt buộc)
    // Đây là phần logic riêng của Tầm xa
    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (animator != null && !string.IsNullOrEmpty(animationName))
            animator.Play(animationName);

        // Chờ cho animation chạy xong
        yield return new WaitForSeconds(attackAnimationDuration);

        // Logic bắn đạn
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Chưa gán Projectile Prefab hoặc Fire Point!");
        }

        isAttacking = false;
        enemy.ChangeState(enemy.idleState);
    }
}