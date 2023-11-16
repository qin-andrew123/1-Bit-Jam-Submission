using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAbilityComp : MonoBehaviour
{
    //This is for debugging feel free to remove this SF 
    [SerializeField] private FOVMeshComp _fovMeshComp;
    [SerializeField] private float decreaseAngle = 5.0f;
    [SerializeField] private float shrinkTime = 2.0f;
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
        _outerAngle -= decreaseAngle;
        //_lightComp.pointLightOuterAngle = Mathf.SmoothStep(_lightComp.pointLightOuterAngle, _outerAngle, 1f);
        StartCoroutine(ShrinkLight(shrinkTime));
        if (_fovMeshComp != null)
            _fovMeshComp.SetFOV(_outerAngle);
    }

    IEnumerator ShrinkLight(float shrinkTime) {
        float elapsedTime = 0.0f;
        while (elapsedTime < shrinkTime) {
            _lightComp.pointLightOuterAngle = Mathf.SmoothStep(_lightComp.pointLightOuterAngle, _outerAngle, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
