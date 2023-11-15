using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem _inputSystem;
    private InputAction _movement;
    [HideInInspector] public Vector2 _moveInput;
    [SerializeField] private float _movementSpeed = 1f;
    
    private void Start()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Player.Enable();
        _movement = _inputSystem.Player.Movement;
    }

    private void Update()
    {
        _moveInput = _movement.ReadValue<Vector2>();
        _moveInput.Normalize();
        // Move that hoe
        gameObject.transform.Translate(_moveInput * _movementSpeed * Time.deltaTime);
    }
}
