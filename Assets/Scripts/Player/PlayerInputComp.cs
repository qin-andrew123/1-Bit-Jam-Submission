using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerInputComp : MonoBehaviour
{
    [SerializeField][Tooltip("DO NOT MODIFIY THIS IN EDITOR")]
    private bool _isAbilityUsed = false;
    [SerializeField]
    private float _skillCooldown = 3.0f;
    private float _timer = 0.0f;
    public event EventHandler OnLeftClick;
    private List<GameObject> _enemiesFound;
    [SerializeField] private int _numRays;
    [SerializeField] private float _castLength;
    void Start()
    {
        _timer = _skillCooldown;
    }
    private void FixedUpdate()
    {
        if(_isAbilityUsed)
        {
            _timer -= Time.fixedDeltaTime;
            if(_timer <= 0 )
            {
                _isAbilityUsed = false;
                _timer = _skillCooldown;
            }
        }
        else if (!_isAbilityUsed && Input.GetMouseButtonDown(0))
        {
            OnLeftClick?.Invoke(this, EventArgs.Empty);
            _isAbilityUsed = true;
            SearchEnemies();
        }
    }

    private void SearchEnemies()
    {
        Light2D playerLight = GetComponentInChildren<Light2D>();
        float lightAngle = playerLight.pointLightOuterAngle / 2.0f;
        float angleIncrement = lightAngle / _numRays;
        for(int i = 0; i <= _numRays; ++i)
        {
            float negativeCurrentAngle = -(i * angleIncrement);
            float positiveCurrentAngle = i * angleIncrement;
            Vector3 mouseToCenter = Input.mousePosition - transform.position;
            mouseToCenter.Normalize();
            Vector3 negativeRay = mouseToCenter * negativeCurrentAngle;
            Vector3 positiveRay = mouseToCenter * positiveCurrentAngle;
            RaycastHit2D negativeHit = Physics2D.Raycast(transform.position, negativeRay, _castLength);
            RaycastHit2D positiveHit = Physics2D.Raycast(transform.position, positiveRay, _castLength);
            if(negativeHit.collider != null && negativeHit.rigidbody.CompareTag("Enemy"))
            {
                _enemiesFound.Add(negativeHit.collider.gameObject);
            }
            if (positiveHit.collider != null && positiveHit.rigidbody.CompareTag("Enemy"))
            {
                _enemiesFound.Add(positiveHit.collider.gameObject);
            }
        }

        for(int i = 0; i < _enemiesFound.Count; ++i)
        {
            Debug.Log("Enemy in List: " + _enemiesFound[i].gameObject.transform.position);
        }
    }
}
