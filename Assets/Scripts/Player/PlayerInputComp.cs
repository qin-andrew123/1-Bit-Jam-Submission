using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerInputComp : MonoBehaviour {
    [SerializeField] private int _numOfRaysOneSide = 30;
    [SerializeField] private float _skillCooldown = 3.0f;
    private float _fov;
    [SerializeField] private float _castLength = 4f;
    [SerializeField] private LayerMask _mask;

    private bool _isAbilityUsed = false;
    private float _timer = 0.0f;
    [SerializeField] private List<GameObject> _enemiesFound;

    // for Debug
    private float angle;

    /// <summary>
    /// Initialize vars
    /// </summary>
    void Start() {
        _timer = _skillCooldown;
        _enemiesFound = new List<GameObject>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, angle) * transform.right * _castLength);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -angle) * transform.right * _castLength);
    }


    /// <summary>
    /// Updates and casts rays after data is sent from PlayerController
    /// </summary>
    private void LateUpdate() {
        if (!_isAbilityUsed && Input.GetMouseButtonDown(0)) {
            // Enemy kill logic
            _enemiesFound.Clear();
            _fov = GetComponentInChildren<Light2D>().pointLightOuterAngle;
            float angleIncrease = _fov / _numOfRaysOneSide / 2.0f;
            angle = 0.0f;
            for (int i = 0; i < _numOfRaysOneSide; i++) {
                RaycastHit2D RightHit = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, angle) * transform.right, _castLength, _mask);
                RaycastHit2D LeftHit = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -angle) * transform.right, _castLength, _mask);

                if (RightHit.collider != null) {
                    if (!_enemiesFound.Contains(RightHit.collider.gameObject)) {
                        _enemiesFound.Add(RightHit.collider.gameObject);
                    }
                }
                if (LeftHit.collider != null) {
                    if (!_enemiesFound.Contains(LeftHit.collider.gameObject)) {
                        _enemiesFound.Add(LeftHit.collider.gameObject);
                    }
                }
                angle += angleIncrease;
            }
            LightAbilityComp playerLight = GetComponentInChildren<LightAbilityComp>();
            if (playerLight == null) {
                Debug.Log("PLAYERABILITY LIGHT IS NULL");
            } else {
                playerLight.Clap();
            }
            _isAbilityUsed = true;

            //Do something with the enemies
            for (int i = 0; i < _enemiesFound.Count; i++) {
                Debug.Log("enemy: " + _enemiesFound[i].gameObject.transform);
            }
        }
    }

    private void FixedUpdate() {
        if (_isAbilityUsed) {
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0) {
                _isAbilityUsed = false;
                _timer = _skillCooldown;
            }
        }
    }
}
