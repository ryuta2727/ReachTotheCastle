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
    [SerializeField]
    CharaDataBase playerStatus;
    [SerializeField]
    GameObject sword;

    private Rigidbody rbody;
    private Quaternion targetRotation;

    //private float speed = 0;
    private bool playerCanMove = true;
    private bool playerDodge = false;

    private Vector2 playerMove = Vector2.zero;

    private void Awake()
    {
        targetRotation = transform.rotation;
        rbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移動関連
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var rotationSpeed = 400 * Time.deltaTime;
        if (playerCanMove)
        {
            var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
            rbody.velocity = horizontalRotation * new Vector3(playerMove.x * playerSpeed * speed, 0, playerMove.y * playerSpeed * speed);
            anim.SetFloat("Speed", rbody.velocity.normalized.magnitude * speed, 0.1f, Time.deltaTime);
        }
        else
        {
            rbody.velocity = Vector3.zero;
        }

        if (rbody.velocity.normalized.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(rbody.velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
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
            playerCanMove = false;
            sword.tag = "Sword";
            anim.SetTrigger("Atack");
        }
    }
    public void OnFire2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetTrigger("Atack2");
        }
    }
    public void OnFireRush(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetTrigger("Atack3");
        }
    }
    public void OnGuard(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetBool("Guard",true);
        }
        if(context.canceled)
        {
            anim.SetBool("Guard", false);
        }
    }
    public void OnMagic(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetTrigger("Magic");
        }
    }
    public void PlayerCanMove()
    {
        playerCanMove = true;
    }
    public int PlayerAtk()
    {
        return PlayerManager.Instance.PlayerAtk();
    }
    public void PlayerDeath()
    {
        playerCanMove = false;
        anim.SetTrigger("Death");
    }
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        playerDodge = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("EnemyAttack") && !playerDodge)
        {
            playerDodge = true;
            anim.SetTrigger("Damaged");
            StartCoroutine(DamagedCoolTime());
            if(PlayerManager.Instance.PlayerDamaged(GameManager.Instance.status.charaList[collision.transform.root.GetComponent<EnemyContoroller>().EnemyNumber()].Atk) <= 0)
            {
                PlayerDeath();
            }
        }
    }
}
