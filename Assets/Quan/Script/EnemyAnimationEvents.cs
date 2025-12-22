using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyController controller;

    void Start()
    {
        // Lấy reference Controller từ object cha
        controller = GetComponentInParent<EnemyController>();
    }

    // Hàm này bạn sẽ chọn trong cửa sổ Animation Event
    public void TriggerAttackBox(int active)
    {
        if (controller != null && controller.attackState != null)
        {
            // Ép kiểu để gọi đúng hàm trong EnemyAttackState
            EnemyAttackState attackState = controller.attackState as EnemyAttackState;
            if (attackState != null)
            {
                // active == 1 thì bật (true), ngược lại thì tắt (false)
                attackState.HandleAttackBoxEvent(active == 1);
            }
        }
    }
}