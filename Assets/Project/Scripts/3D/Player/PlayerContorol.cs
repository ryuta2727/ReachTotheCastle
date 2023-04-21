using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContorol : MonoBehaviour
{
    //プレイヤーのスピード調整
    [SerializeField]
    private float playerSpeed = 3;
    [SerializeField]
    Animator anim;
    private Rigidbody rbody;
    private Quaternion targetRotation;

    //private float speed = 0;

    private Vector3 playerMove = Vector3.zero;
    public Vector3 PlayerMove { get => playerMove; }

    private void Awake()
    {
        targetRotation = transform.rotation;
        rbody = GetComponent<Rigidbody>();
        //anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移動関連
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var rotationSpeed = 600 * Time.deltaTime;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        rbody.velocity = horizontalRotation * new Vector3(playerMove.x * playerSpeed, 0, playerMove.y * playerSpeed);
        if (rbody.velocity.normalized.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.LookRotation(rbody.velocity.normalized, Vector3.up);
        }
        anim.SetFloat("Speed", rbody.velocity.normalized.magnitude * speed, 0.1f, Time.deltaTime);
        //

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
        //Debug.Log(playerMove);
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            anim.SetTrigger("Atack");
        }
    }
    public void OnShift(InputAction.CallbackContext context)
    {

    }
}
