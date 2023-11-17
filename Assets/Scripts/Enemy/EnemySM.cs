using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySM : StateMachine
{
    public Vector2 PlayerLocation { get; private set; }

    [HideInInspector] public EnemyIdle idleState;
    [HideInInspector] public EnemyMoving movingState;
    [HideInInspector] public EnemyDie dieState;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _time = 3.0f;
    private bool _hasCollided = false;
    private float _timer = 0.0f;

    private void Awake()
    {
        idleState = new EnemyIdle(this);
        movingState = new EnemyMoving(this, _moveSpeed);
        dieState = new EnemyDie(this);
    }
    protected override BaseState GetInitialState()
    {
        return idleState;
    }
    private void FixedUpdate()
    {
        if(_hasCollided)
        {
            _timer += Time.fixedDeltaTime;
            if(_timer > _time)
            {
                _hasCollided = false;
                _timer = 0.0f;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_hasCollided && collision.CompareTag("Player"))
        {
            PlayerLocation = collision.transform.position;
            ChangeState(movingState);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeState(idleState);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _hasCollided = true;
            ChangeState(idleState);
        }
    }

    private void OnDestroy()
    {
        
    }
}
