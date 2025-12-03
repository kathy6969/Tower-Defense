using UnityEngine;
using System.Collections;

// Kế thừa từ BaseAttackState là đúng
public class EnemyRangedAttackState : BaseAttackState
{
    [Header("Cấu hình Tầm xa")]
    public GameObject projectilePrefab; // Prefab đạn
    public Transform firePoint;        // Vị trí bắn

    // Logic OnEnter, OnUpdate, OnExit được thừa kế từ BaseAttackState/EnemyState

    // Định nghĩa hàm AttackRoutine (bắt buộc)
    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // ⚠️ SỬA ĐỔI 1: Truy cập Animator thông qua enemy (Controller)
        if (enemy.animator != null && !string.IsNullOrEmpty(animationName))
            enemy.animator.Play(animationName); // Thay vì 'animator.Play'

        // Chờ cho animation chạy xong
        yield return new WaitForSeconds(attackAnimationDuration);

        // Logic bắn đạn
        if (projectilePrefab != null && firePoint != null)
        {
            // ⚠️ SỬA ĐỔI 2: Instantiate cần được gọi bởi một MonoBehaviour, 
            // có thể gọi từ Controller (enemy) hoặc chính State này (this)
            // Gọi từ Controller là hợp lý hơn.
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