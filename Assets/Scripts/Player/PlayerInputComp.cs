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
    [SerializeField] private float ShrinkAngleWhenClap = 5f;
    private float _fov;
    private bool _isAbilityUsed = false;
    private float _timer = 0.0f;
    private Light2D _light;
    [SerializeField] private GameObject hintText;
    [SerializeField] private AudioManager audioManager;


    // for Debug
    private float angle;

    /// <summary>
    /// Initialize vars
    /// </summary>
    void Start() {
        _timer = _skillCooldown;
        _enemiesFound = new List<GameObject>();
        _light = GetComponentInChildren<Light2D>();
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
            audioManager.Play("Flashlight");
            _fov = _light.pointLightOuterAngle;
            float angleIncrease = _fov / _numOfRaysOneSide / 2.0f;
            angle = 0.0f;
            for (int i = 0; i < _numOfRaysOneSide; i++) {
                RaycastHit2D[] RightHit = Physics2D.RaycastAll(transform.position, Quaternion.Euler(0, 0, angle) * transform.right, _castLength, _mask);
                RaycastHit2D[] LeftHit = Physics2D.RaycastAll(transform.position, Quaternion.Euler(0, 0, -angle) * transform.right, _castLength, _mask);

                for (int j = 0; j < RightHit.Length; j++) {
                    RaycastHit2D hit = RightHit[j];
                    if (hit.collider != null) {
                        if (!_enemiesFound.Contains(hit.collider.gameObject)) {
                            _enemiesFound.Add(hit.collider.gameObject);
                        }
                    }
                }
                for (int j = 0; j < LeftHit.Length; j++) {
                    RaycastHit2D hit = LeftHit[j];
                    if (hit.collider != null) {
                        if (!_enemiesFound.Contains(hit.collider.gameObject)) {
                            _enemiesFound.Add(hit.collider.gameObject);
                        }
                    }
                }
                angle += angleIncrease;
            }
            LightAbilityComp playerLight = GetComponentInChildren<LightAbilityComp>();
            if (playerLight == null) {
                Debug.Log("PLAYERABILITY LIGHT IS NULL");
            } else {
                _light.gameObject.SetActive(false);
                Invoke("EnableLight", 0.3f);
                playerLight.Clap(ShrinkAngleWhenClap); // Shrink amount
                _light.pointLightOuterAngle = playerLight.GetOuterAngle();
            }
            _isAbilityUsed = true;

            //Do something with the enemies
            KillEnemies();

        }
    }

    private void KillEnemies() {
        for (int i = 0; i < _enemiesFound.Count; ++i) {
            Destroy(_enemiesFound[i]);
            GameManager.EnemiesDestroyedByAttack++;
        }
        Debug.Log("EnemiesDestroyedByAttack: " + GameManager.EnemiesDestroyedByAttack);
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
        if (_isAbilityUsed) {
            hintText.SetActive(false);
        } else {
            hintText.SetActive(true);
        }
    }

    private void EnableLight() {
        _light.gameObject.SetActive(true);
    }
}
