using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    public InputAction moveAction;
    public float moveSpeed = 10.0f;
    public Vector2 position;
    void Start()
    {
        moveAction.Enable();
    }

    void Update()
    {
        var moveDirection = moveAction.ReadValue<Vector2>();
        position += moveDirection * moveSpeed * Time.deltaTime;
        if(Mouse.current.leftButton.isPressed)
        {

        }
    }
}
