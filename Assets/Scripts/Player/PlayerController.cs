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
    [SerializeField] private bool _isRotateLerp = true;
    [SerializeField] private float _rotateSpeed = 5f;
    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private PlayerInputComp _playerInputComp;
    //This is for Debugging Purpose. Feel free to remove this SF
    [SerializeField] private FOVMeshComp _fovMeshComp;
    private void OnDrawGizmos() {
        if (!_showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(this.transform.position, transform.right * 2);
    }

    private void Start()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Player.Enable();
        _movement = _inputSystem.Player.Movement;
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandlePlayerRotation();
    }

    private void HandlePlayerMovement() {
        _moveInput = _movement.ReadValue<Vector2>();
        _moveInput.Normalize();
        gameObject.transform.Translate(_moveInput * _movementSpeed * Time.deltaTime, Space.World);
        _playerInputComp.SetOrigin(transform.position);
        _fovMeshComp.SetOrigin(transform.position);
    }

    private void HandlePlayerRotation() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 newDirection = mouseWorld - transform.position;
        _playerInputComp.SetAimDirection(newDirection);
        _fovMeshComp.SetAimDirection(newDirection);
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        Quaternion newRotationDirection = Quaternion.AngleAxis(angle, Vector3.forward);
        if (_isRotateLerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotationDirection, _rotateSpeed * Time.deltaTime);
        else {
            transform.rotation = newRotationDirection;
        }
    }
}
