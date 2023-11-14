using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    public float MovementSpeed {  get { return _movementSpeed; } set {  _movementSpeed = value; } }
    
    private void Start()
    {
    }

    private void Update()
    {
        // player input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        // Calculate movement 
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
        // Move that hoe
        transform.Translate(movement * MovementSpeed * Time.deltaTime, Space.World);
    }
}
