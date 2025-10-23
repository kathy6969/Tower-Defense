using UnityEngine;

// Tên file phải là: EnemyChaseState.cs
public class EnemyMoveState : EnemyState
{
    // Đây là biến từ code 'move' của bạn
    public float moveSpeed = 2f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 1. Kiểm tra mục tiêu (Lấy từ code 'move' của bạn)
        if (enemy.targetPlayer == null)
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        // 2. Kiểm tra chuyển State (Đây là LOGIC MỚI đã sửa)

        // Nếu vào tầm tấn công
        if (enemy.PlayerInAttackRange())
        {
            // Logic lựa chọn MỚI (sửa lỗi nhấp nháy)
            if (enemy.attackState.IsAttackReady())
            {
                enemy.ChangeState(enemy.attackState); // Sẵn sàng -> Tấn công
            }
            else
            {
                enemy.ChangeState(enemy.idleState); // Chưa sẵn sàng -> Đứng chờ
            }
            return; // Thoát
        }

        // Nếu mất dấu (Logic MỚI)
        if (!enemy.PlayerInDetectRange())
        {
            enemy.targetPlayer = null;
            enemy.ChangeState(enemy.idleState);
            return;
        }

        // 3. Logic Di Chuyển (Đây là code 'move' CŨ của bạn)
        // Nếu không chuyển state, thì tiếp tục di chuyển
        Vector2 dir = (enemy.targetPlayer.position - enemy.centerPoint.position).normalized;
        enemy.transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        // (Bạn có thể thêm code lật sprite ở đây nếu cần)
    }
}