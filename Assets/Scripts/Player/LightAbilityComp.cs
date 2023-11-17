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

    public void Clap(float decreaseAmount)
    {
        Debug.Log("Light Clapped");
        _outerAngle -= decreaseAmount;
        //_lightComp.pointLightOuterAngle = Mathf.SmoothStep(_lightComp.pointLightOuterAngle, _outerAngle, 1f);
        StartCoroutine(ShrinkLight(2.0f));
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

    public float GetOuterAngle()
    {
        return _outerAngle;
    }
}
