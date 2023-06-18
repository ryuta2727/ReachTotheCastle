using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContorol : MonoBehaviour
{

    //�v���C���[�̃X�s�[�h����
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
    private bool isPlayerGeard = false;
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
        //--�ړ��֘A--
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
        //�^�[������Ƃ��������炩��
        if (rbody.velocity.normalized.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(rbody.velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }
    //InputAction/WASD
    public void OnMove(InputAction.CallbackContext context)
    {
        //�ړ��̃x�N�g���Z�o
        playerMove = context.ReadValue<Vector2>();
    }
    //InputAction/���N���b�N
    public void OnFire(InputAction.CallbackContext context)
    {
        //--�U��
        //��d�ǂݍ��ݖh�~
        if(context.performed)
        {
            //�U�����͈ړ��ł��Ȃ�
            playerCanMove = false;
            //���̃^�O��L����
            sword.tag = "Sword";
            anim.SetTrigger("Atack");
        }
    }
    //InputAction/�E�N���b�N
    public void OnFire2(InputAction.CallbackContext context)
    {
        //--�U��2
        //��d�ǂݍ��ݖh�~
        if (context.performed)
        {
            //�U�����͈ړ��ł��Ȃ�
            playerCanMove = false;
            //���̃^�O��L����
            sword.tag = "Sword";
            anim.SetTrigger("Atack2");
        }
    }
    //InputAction/Ctrl
    public void OnGuard(InputAction.CallbackContext context)
    {
        //--�K�[�h
        if (context.performed)
        {
            isPlayerGeard = true;
            //�K�[�h���͓����Ȃ�
            playerCanMove = false;
            anim.SetBool("Guard",true);
        }
        //�K�[�h����
        if(context.canceled)
        {
            isPlayerGeard = false;
            anim.SetBool("Guard", false);
        }
    }
    //InputAction/C
    public void OnMagic(InputAction.CallbackContext context)
    {
        //���ɈӖ��̂Ȃ����[�V����
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetTrigger("Magic");
        }
    }
    //�v���C���[���ړ��\��Ԃ�
    public void PlayerCanMove()
    {
        playerCanMove = true;
    }
    //�v���C���[�̍U���͂�Ԃ�
    public int PlayerAtk()
    {
        return PlayerManager.Instance.PlayerAtk();
    }
    //�v���C���[���S
    public void PlayerDeath()
    {
        playerCanMove = false;
        anim.SetTrigger("Death");
        //�V�[�����Z�b�g����
        GameManager.Instance.SceneResetGameOver();
    }
    //��_���̃N�[���^�C��
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        playerDodge = false;
    }
    //��_�����m
    private void OnTriggerEnter(Collider collision)
    {
        //EnemyAttackTag�������ꍇ
        if (collision.CompareTag("EnemyAttack") && !playerDodge && !isPlayerGeard)
        {
            //�v���C���[�𖳓G��Ԃ�
            playerDodge = true;
            anim.SetTrigger("Damaged");
            //��_���̃N�[���^�C������
            StartCoroutine(DamagedCoolTime());
            //0�ȉ��ɂȂ����ꍇ
            if(PlayerManager.Instance.PlayerDamaged(GameManager.Instance.status.charaList[collision.transform.root.GetComponent<EnemyContoroller>().EnemyNumber()].Atk) <= 0)
            {
                //���S
                PlayerDeath();
            }
        }
    }
}
