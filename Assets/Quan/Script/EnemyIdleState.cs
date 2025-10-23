using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        // Ưu tiên 1: Kiểm tra xem có thể tấn công không?
        // Player CÒN trong tầm VÀ đòn đánh ĐÃ sẵn sàng?
        if (enemy.PlayerInAttackRange() && enemy.attackState.IsAttackReady())
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        // Ưu tiên 2: Player chạy khỏi tầm đánh, nhưng vẫn trong tầm phát hiện?
        if (!enemy.PlayerInAttackRange() && enemy.PlayerInDetectRange())
        {
            enemy.GetPlayerTarget();
            enemy.ChangeState(enemy.EnemyMoveState); // 👈 Đã sửa tên
            return;
        }

        // Nếu không (Player chạy mất, hoặc Player trong tầm nhưng đang cooldown)
        // thì không làm gì cả, tiếp tục đứng im (Idle).
    }
}