using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void OnUpdate()
    {
        if (enemy.PlayerInDetectRange())
        {
            enemy.GetPlayerTarget();
            enemy.ChangeState(enemy.moveState);
        }
    }
}
