using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputComp : MonoBehaviour
{
    [SerializeField][Tooltip("DO NOT MODIFIY THIS IN EDITOR")]
    private bool _isAbilityUsed = false;
    [SerializeField]
    private float _skillCooldown = 3.0f;
    private float _timer = 0.0f;
    //Get component for Ability

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
            Debug.Log("Ability is used");
            _isAbilityUsed = true;
        }
    }
}
