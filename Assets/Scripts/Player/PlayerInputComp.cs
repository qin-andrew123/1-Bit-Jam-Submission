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
    [SerializeField] private float _castLength = 4f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private List<GameObject> _enemiesFound;
    private float _fov;
    private bool _isAbilityUsed = false;
    private float _timer = 0.0f;

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
                playerLight.Clap(5.0f); // Shrink amount
            }
            _isAbilityUsed = true;

            //Do something with the enemies
            KillEnemies();
        }
    }

    private void KillEnemies()
    {
        for(int i = 0; i < _enemiesFound.Count; ++i)
        {
            Destroy(_enemiesFound[i]);
        }
        _enemiesFound.Clear();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider != null && collision.collider.CompareTag("Enemy"))
        {
            LightAbilityComp playerLight = GetComponentInChildren<LightAbilityComp>();
            playerLight.Clap(10.0f);
        }    
    }
}
