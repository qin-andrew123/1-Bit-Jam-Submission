using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected BaseState currentState;
    private void Start()
    {
        currentState = GetInitialState();
        if(currentState != null)
        {
            currentState.Enter();
        }
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState.UpdateLogic();
        }
    }
    private void LateUpdate()
    {
        if(currentState != null)
        {
            currentState.UpdatePhysics();
        }
    }

    public void ChangeState(BaseState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    private void OnGUI()
    {
        string content = currentState != null ? currentState.name : "No Current State";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }
}
