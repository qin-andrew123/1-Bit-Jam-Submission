using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerInputComp : MonoBehaviour
{
    [SerializeField] private int _numRays;
    [SerializeField] private float _skillCooldown = 3.0f;
    [SerializeField] private float _fov;
    [SerializeField] private float _castLength;
    [SerializeField] private LayerMask _mask;

    private Vector3 _origin;
    private float _startingAngle = 0.0f;
    private bool _isAbilityUsed = false;
    private float _timer = 0.0f;
    private List<GameObject> _enemiesFound;

    /// <summary>
    /// Initialize vars
    /// </summary>
    void Start()
    {
        _timer = _skillCooldown;
        _enemiesFound = new List<GameObject>();
        _origin = Vector3.zero;
    }

    /// <summary>
    /// Updates and casts rays after data is sent from PlayerController
    /// </summary>
    private void LateUpdate()
    {
        _fov = GetComponentInChildren<Light2D>().pointLightOuterAngle;
        float angle = _startingAngle;
        float angleIncrease = _fov / _numRays;
        if (!_isAbilityUsed && Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i <= _numRays; i++)
            {
                float angleRad = angle * Mathf.Deg2Rad;
                RaycastHit2D hit = Physics2D.Raycast(_origin, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), _castLength, _mask);
                if (hit.collider != null)
                {
                    if (!_enemiesFound.Contains(hit.collider.gameObject))
                    {
                        _enemiesFound.Add(hit.collider.gameObject);
                    }
                }
                angle -= angleIncrease;
            }
            LightAbilityComp playerLight = GetComponentInChildren<LightAbilityComp>();
            if(playerLight == null)
            {
                Debug.Log("PLAYERABILITY LIGHT IS NULL");
            }
            else
            {
                playerLight.Clap();
            }
            _isAbilityUsed = true;

            //Do something with the enemies
            for(int i = 0; i < _enemiesFound.Count; i++)
            {
                Debug.Log("enemy: " + _enemiesFound[i].gameObject.transform);
            }
            _enemiesFound.Clear();
        }
    }

    private void FixedUpdate()
    {
        if (_isAbilityUsed)
        {
            _timer -= Time.fixedDeltaTime;
            if(_timer <= 0 )
            {
                _isAbilityUsed = false;
                _timer = _skillCooldown;
            }
        }
    }
    public void SetOrigin(Vector3 origin)
    {
        _origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        aimDirection.Normalize();
        _startingAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if(_startingAngle < 0 )
        {
            _startingAngle += 360.0f;
        }

        _startingAngle += _fov / 2f;
    }
}
