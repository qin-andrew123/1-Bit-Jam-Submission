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
    private bool _isAbilityUsed = false;
    [SerializeField]
    private float _skillCooldown = 3.0f;
    private float _timer = 0.0f;
    public event EventHandler OnLeftClick;
    private List<GameObject> _enemiesFound;
    [SerializeField] private int _numRays;
    [SerializeField] private float _castLength;
    private Mesh mesh;

    private float _fov;
    void Start()
    {
        _timer = _skillCooldown;
        _enemiesFound = new List<GameObject>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3 origin = Vector3.zero;
        float angle = 0f;
        float angleIncrease = _fov / _numRays;

        Vector3[] vertices = new Vector3[_numRays + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[_numRays * 3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int triangleIndex = 1;
        for(int i = 0; i <= _numRays; i++)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 vertex = origin + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * _castLength;
            vertices[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex -1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            ++vertexIndex;
            angle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
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
            _enemiesFound.Clear();
        }
    }

    private void SearchEnemies()
    {
        Light2D playerLight = GetComponentInChildren<Light2D>();
        float lightAngle = playerLight.pointLightOuterAngle / 2.0f;
        float angleIncrement = lightAngle / _numRays;
        for (int i = 0; i <= _numRays; ++i)
        {
            
            float currentAngle = i * angleIncrement; 
            Vector3 mouseToCenter = Input.mousePosition - transform.position;
            mouseToCenter.Normalize();

            float negX = transform.position.x - Mathf.Sin(-currentAngle) * Mathf.Deg2Rad;
            float negY = transform.position.y - Mathf.Cos(-currentAngle) * Mathf.Deg2Rad;
            float posX = transform.position.x + Mathf.Sin(currentAngle) * Mathf.Deg2Rad;
            float posY = transform.position.y + Mathf.Cos(currentAngle) * Mathf.Deg2Rad;
            negativeRay = new Vector2(negX, negY);
            negativeRay.Normalize();
            positiveRay = new Vector2(posX, posY);
            positiveRay.Normalize();
            
            RaycastHit2D negativeHit = Physics2D.Raycast(transform.position, negativeRay, _castLength);
            RaycastHit2D positiveHit = Physics2D.Raycast(transform.position, positiveRay, _castLength);
            if (negativeHit && negativeHit.rigidbody.CompareTag("Enemy"))
            {
                if(!_enemiesFound.Contains(negativeHit.collider.gameObject))
                {
                    _enemiesFound.Add(negativeHit.collider.gameObject);
                    Debug.Log("Negative Ray Found enemy. NegativeRay: " + negativeRay);
                    Debug.Log("Forward: " + mouseToCenter);
                }
            }
            if (positiveHit && positiveHit.rigidbody.CompareTag("Enemy"))
            {
                if(!_enemiesFound.Contains(positiveHit.collider.gameObject))
                {
                    Debug.Log("PositiveRay Found enemy: " + positiveRay);
                    _enemiesFound.Add(positiveHit.collider.gameObject);
                    Debug.Log("Forward: " + mouseToCenter);
                }
            }   
        }

        for (int i = 0; i < _enemiesFound.Count; ++i)
        {
            Debug.Log("Enemy in List: " + _enemiesFound[i].gameObject.transform.position);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, negativeRay * _castLength);
        Gizmos.DrawRay(transform.position, positiveRay * _castLength);
    }

}
