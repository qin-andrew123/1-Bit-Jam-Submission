using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeShaderPivot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float xScale, yScale;
    //public float maxDis;
    void Start()
    {
        GameObject curPlayer = GameObject.FindGameObjectWithTag("Player");
        if (curPlayer != null)
            player = curPlayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toPlayer = transform.position - player.transform.position;
        transform.up = toPlayer;

        float dis = toPlayer.magnitude;
        dis = Mathf.Max(0.8f,Mathf.Min(dis, 2));
        Vector3 scale = new Vector3(dis * xScale, 1/dis * yScale, 1);
        transform.localScale = scale;
    }
}
