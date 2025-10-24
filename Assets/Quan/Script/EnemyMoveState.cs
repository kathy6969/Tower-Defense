using UnityEngine;

// Tên file phải là: EnemyMoveState.cs
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

        // 2. Kiểm tra chuyển State

        // Nếu vào tầm tấn công
        if (enemy.PlayerInAttackRange())
        {
            // Logic lựa chọn MỚI (sửa lỗi nhấp nháy)

            // ⚠️ SỬA ĐỔI QUAN TRỌNG:
            // Gọi hàm IsAttackReady() từ 'enemy' (Controller)
            // thay vì từ 'enemy.attackState'
            if (enemy.IsAttackReady()) // 👈 ĐÃ SỬA DÒNG NÀY
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
        Vector2 dir = (enemy.targetPlayer.position - enemy.centerPoint.position).normalized;
        enemy.transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        // 4. GỌI HÀM LẬT SPRITE (Đúng rồi)
        enemy.CheckAndFlip(dir);
    }
}