using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public float moveSpeed = 2f;

    public override void OnUpdate()
    {
        if (enemy.targetPlayer == null)
        {
            enemy.ChangeState(enemy.idleState);
            return;
        }

        if (enemy.PlayerInAttackRange())
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        // Di chuyển tới player
        Vector2 dir = (enemy.targetPlayer.position - enemy.centerPoint.position).normalized;
        enemy.transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }
}
