using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : BaseState
{
    private float moveSpeed;
    public EnemyMoving(EnemySM stateMachine, float moveSpeed) : base("EnemyMoving", stateMachine)
    {
        this.moveSpeed = moveSpeed;
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
        stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position, ((EnemySM)stateMachine).PlayerLocation, moveSpeed * Time.deltaTime);
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
