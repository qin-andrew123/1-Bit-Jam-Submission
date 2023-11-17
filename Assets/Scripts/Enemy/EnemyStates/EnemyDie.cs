using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : BaseState
{
    public EnemyDie(EnemySM stateMachine) : base("DieState", stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
