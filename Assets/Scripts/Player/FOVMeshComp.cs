using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVMeshComp : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private Mesh _mesh;
    private float _fov;
    private Vector3 _origin;
    private float _startingAngle;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _origin = Vector3.zero;
        _fov = 100f;
    }

    private void LateUpdate()
    {
        int numRays = 50;
        float angle = _startingAngle;
        float angleIncrease = _fov / numRays;
        float viewDistance = 5.0f;

        Vector3[] vertices = new Vector3[numRays + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[numRays * 3];

        vertices[0] = _origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= numRays; i++) 
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(_origin, new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)), viewDistance, _layerMask);
            if(hit.collider == null)
            {
                vertex = _origin + new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }
            vertices[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;  
        _mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this._origin = origin;
    }
    public void SetAimDirection(Vector3 aimDirection)
    {
        aimDirection.Normalize();
        _startingAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (_startingAngle < 0)
        {
            _startingAngle += 360.0f;
        }

        _startingAngle += _fov / 2f;
    }
    public void SetFOV(float fov)
    {
        _fov = fov;
    }
}
