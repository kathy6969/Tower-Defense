using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [Header("Animation")]
    public string animationName;
    // ❌ ĐÃ XÓA: public Animator animator;

    protected EnemyController enemy;

    public virtual void OnEnter(EnemyController enemy)
    {
        this.enemy = enemy;

        // 👉 THÊM: Thiết lập tốc độ Animator về tốc độ MẶC ĐỊNH
        // Logic này đảm bảo Idle/Move luôn dùng tốc độ chuẩn, 
        // và reset tốc độ sau khi rời AttackState.
        enemy.SetAnimatorSpeed(enemy.animatorSpeed);

        // ✔ Dùng animator từ EnemyController (đúng mô hình FSM)
        if (enemy.animator != null && !string.IsNullOrEmpty(animationName))
            enemy.animator.Play(animationName);
    }

    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}