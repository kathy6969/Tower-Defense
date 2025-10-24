using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void OnUpdate()
    {
        base.OnUpdate();

        // Ưu tiên 1: Kiểm tra xem có thể tấn công không?

        // ⚠️ SỬA ĐỔI QUAN TRỌNG:
        // Gọi hàm IsAttackReady() từ 'enemy' (Controller)
        // thay vì từ 'enemy.attackState'
        if (enemy.PlayerInAttackRange() && enemy.IsAttackReady()) // 👈 ĐÃ SỬA DÒNG NÀY
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        // Ưu tiên 2: Player chạy khỏi tầm đánh, nhưng vẫn trong tầm phát hiện?
        if (!enemy.PlayerInAttackRange() && enemy.PlayerInDetectRange())
        {
            enemy.GetPlayerTarget();
            enemy.ChangeState(enemy.moveState);
            return;
        }

        // Nếu không (Player chạy mất, hoặc Player trong tầm nhưng đang cooldown)
        // thì không làm gì cả, tiếp tục đứng im (Idle).
    }
}