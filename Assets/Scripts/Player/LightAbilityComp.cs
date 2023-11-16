using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAbilityComp : MonoBehaviour
{
    //This is for debugging feel free to remove this SF 
    [SerializeField] private FOVMeshComp _fovMeshComp;
    private Light2D _lightComp;
    private float _outerAngle;
    
    void Start()
    {
        _lightComp = GetComponent<Light2D>();
        _outerAngle = _lightComp.pointLightOuterAngle;
    }

    public void Clap()
    {
        Debug.Log("Light Clapped");
        _outerAngle -= 5.0f;
        _lightComp.pointLightOuterAngle = _outerAngle;
        _fovMeshComp.SetFOV(_outerAngle);
    }
}
