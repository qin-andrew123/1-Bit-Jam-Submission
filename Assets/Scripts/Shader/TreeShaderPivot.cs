using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeShaderPivot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float xScale, yScale;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toPlayer = transform.position - player.transform.position;
        transform.up = toPlayer;

        float dis = toPlayer.magnitude;
        Vector3 scale = new Vector3(1/dis * xScale, dis * yScale, 1);
        transform.localScale = scale;
    }
}
