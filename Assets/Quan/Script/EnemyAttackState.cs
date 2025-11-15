using UnityEngine;
using System.Collections;

// ⚠️ SỬA 1: Đổi lớp kế thừa (Giữ nguyên)
public class EnemyAttackState : BaseAttackState // 👈 Đổi từ EnemyState
{
    [Header("Cấu hình Cận chiến")]
    public float enableHitboxTime = 0.45f;
    public float disableHitboxTime = 0.54f;
    public GameObject attackBox;

    // ----- ĐÃ XÓA: attackCooldown, attackAnimationDuration, isAttacking -----
    // (Vì chúng đã nằm trong lớp cha 'BaseAttackState')

    // ⚠️ ĐÃ THÊM: Biến lưu script hitbox
    private EnemyHitbox hitboxScript;

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy); // 👈 Gọi hàm của lớp cha

        // ⚠️ ĐÃ THÊM: Lấy script hitbox khi vào state
        if (attackBox != null)
        {
            attackBox.SetActive(false);
            hitboxScript = attackBox.GetComponent<EnemyHitbox>();
            if (hitboxScript == null)
            {
                Debug.LogError("Không tìm thấy EnemyHitbox script trên " + attackBox.name + "!");
            }
        }
        else
        {
            Debug.LogError("Chưa gán attackBox (hitbox) cho " + this.name + " trên quái " + enemy.name);
        }
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

        // ⚠️ ĐÃ THÊM: Gán sát thương cho hitbox TRƯỚC KHI BẬT
        if (hitboxScript != null)
        {
            // Lấy sát thương đã tính toán từ Controller
            hitboxScript.damage = enemy.GetCurrentDamage();
        }

        // Logic Hitbox
        yield return new WaitForSeconds(enableHitboxTime);
        if (attackBox != null)
            attackBox.SetActive(true); // 👈 Lúc này OnEnable() của hitbox sẽ chạy

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

        // ⚠️ ĐÃ THÊM: Kiểm tra nếu quái còn sống thì mới về Idle
        // (Nếu Player đánh chết quái giữa lúc đang tấn công)
        if (!enemy.IsDead())
        {
            enemy.ChangeState(enemy.idleState);
        }
    }

    public override void OnExit()
    {
        base.OnExit(); // 👈 Gọi hàm của lớp cha
        if (attackBox != null)
            attackBox.SetActive(false);
    }
}