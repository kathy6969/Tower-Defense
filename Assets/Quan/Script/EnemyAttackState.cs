using UnityEngine;
using System.Collections;

public class EnemyAttackState : BaseAttackState
{
    [Header("Cấu hình Cận chiến")]
    public GameObject attackBox; // Object chứa hitbox

    [Header("Attack Animation Speed")]
    public float attackAnimSpeed = 1.5f;

    private bool allowAnimationEvents = false;

    public override void OnEnter(EnemyController enemy)
    {
        base.OnEnter(enemy);
        enemy.SetAnimatorSpeed(attackAnimSpeed);

        // Reset: Luôn tắt box khi vừa vào trạng thái
        if (attackBox != null)
            attackBox.SetActive(false);

        allowAnimationEvents = true;
    }

    protected override IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (enemy.animator != null && !string.IsNullOrEmpty(animationName))
            enemy.animator.Play(animationName);

        // Đợi hết thời gian animation đã cấu hình
        yield return new WaitForSeconds(attackAnimationDuration);

        isAttacking = false;
        allowAnimationEvents = false;

        // Tắt box sau khi đòn đánh kết thúc
        if (attackBox != null)
            attackBox.SetActive(false);

        enemy.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit();
        enemy.SetAnimatorSpeed(enemy.animatorSpeed);

        allowAnimationEvents = false;
        if (attackBox != null)
            attackBox.SetActive(false);
    }

    // 👉 Hàm nhận lệnh từ Animation Event
    public void HandleAttackBoxEvent(bool active)
    {
        // Chỉ thực hiện nếu đang trong quá trình tấn công hợp lệ
        if (allowAnimationEvents && isAttacking && attackBox != null)
        {
            attackBox.SetActive(active);
        }
    }
}