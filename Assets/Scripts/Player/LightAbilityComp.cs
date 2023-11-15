using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAbilityComp : MonoBehaviour
{
    private Light2D _lightComp;
    private float _outerAngle;
    void Start()
    {
        _lightComp = GetComponent<Light2D>();
        PlayerInputComp _playerInputComp = GetComponentInParent<PlayerInputComp>();
        _playerInputComp.OnLeftClick += Clap;
        _outerAngle = _lightComp.pointLightOuterAngle;
    }

    private void Clap(object sender, EventArgs e)
    {
        Debug.Log("Ability is used");
        _outerAngle -= 5.0f;
        _lightComp.pointLightOuterAngle = _outerAngle;
    }

    private void OnDestroy()
    {
        PlayerInputComp _playerInputComp = GetComponentInParent<PlayerInputComp>();
        _playerInputComp.OnLeftClick -= Clap;
    }
}
