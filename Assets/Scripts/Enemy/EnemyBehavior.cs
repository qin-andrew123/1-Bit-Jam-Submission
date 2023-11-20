using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyBehavior : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private float _moveSpeed;

    private void Start() {
        _player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    private void Update() {
        Vector3 _moveInput = _player.transform.position - gameObject.transform.position;
        gameObject.transform.Translate(_moveInput.normalized * _moveSpeed * Time.deltaTime, Space.World);
        float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg;
        Quaternion newRotationDirection = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = newRotationDirection;
    }
}
