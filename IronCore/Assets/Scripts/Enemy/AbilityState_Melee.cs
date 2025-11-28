using UnityEngine;

public class AbilityState_Melee : EnemyState
{

    private Enemy_Melee enemy; 
    public AbilityState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee; 
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled) 
            stateMachine.ChangeState(enemy.recoveryState);
    }
}
