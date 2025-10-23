using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [Header("Animation")]
    public string animationName;
    public Animator animator;

    protected EnemyController enemy;

    public virtual void OnEnter(EnemyController enemy)
    {
        this.enemy = enemy;

        if (animator != null && !string.IsNullOrEmpty(animationName))
            animator.Play(animationName);
    }

    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}
