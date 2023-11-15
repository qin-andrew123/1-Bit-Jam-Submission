using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionOrbit : MonoBehaviour {
    [SerializeField] Transform orbitOrigin;
    [SerializeField] private Ellipse orbitPath = new Ellipse(500f, 500f);
    [Range(0f, 1f)]
    public float orbitProgress = 0f;
    public float orbitPeriod = 3f;
    public bool orbitActive = true;

    private void Start() {
        SetOrbitingObjectPosition();
    }

    private void Update() {
        HandleCompanionOrbit();
    }

    private void SetOrbitingObjectPosition() {
        Vector2 orbitPos = orbitPath.EvaluateEllipse(orbitProgress);
        this.transform.position = new Vector3(orbitPos.x + orbitOrigin.position.x, orbitPos.y + orbitOrigin.position.y, 0f);
    }

    private void HandleCompanionOrbit() {
        if (!orbitActive) return;
        if (orbitPeriod < 0.1f) {
            orbitPeriod = 0.1f;
        }
        float orbitSpeed = 1f / orbitPeriod;
        orbitProgress += Time.deltaTime * orbitSpeed;
        orbitProgress %= 1f;
        SetOrbitingObjectPosition();
    }
}

[System.Serializable]
public class Ellipse {
    [SerializeField] private float xAxis = 500f;
    [SerializeField] private float yAxis = 500f;

    public Ellipse(float xAxis, float yAxis) {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    public Vector2 EvaluateEllipse(float t) {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * xAxis;
        float y = Mathf.Cos(angle) * yAxis;
        return new Vector2(x, y);
    }

    public void SetEllipseXAxis(float xAxis) {
        this.xAxis = xAxis;
    }

    public void SetEllipseYAxis(float yAxis) {
        this.yAxis = yAxis;
    }

    public float GetEllipseXAxis() {
        return xAxis;
    }

    public float GetEllipseYAxis() {
        return yAxis;
    }
}

