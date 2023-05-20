using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMove : MonoBehaviour
{
    private bool check = true;

    private Vector2 playerMove = Vector2.zero;
    private float playerSpeed = 1;

    Rigidbody2D rbody;
    //Animator anim;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rbody.velocity = new Vector2(playerMove.x * playerSpeed, playerMove.y * playerSpeed);
        if (rbody.velocity.magnitude < 0.5f)
        {
            //anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }
        else
        {
            //anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if(check)
        {
            playerMove = context.ReadValue<Vector2>();
        }
    }
}
