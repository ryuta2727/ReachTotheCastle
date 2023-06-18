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
        //--移動関連--
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
        //ターンするとき少し滑らかに
        if (rbody.velocity.normalized.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(rbody.velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }
    //InputAction/WASD
    public void OnMove(InputAction.CallbackContext context)
    {
        //移動のベクトル算出
        playerMove = context.ReadValue<Vector2>();
    }
    //InputAction/左クリック
    public void OnFire(InputAction.CallbackContext context)
    {
        //--攻撃
        //二重読み込み防止
        if(context.performed)
        {
            //攻撃中は移動できない
            playerCanMove = false;
            //剣のタグを有効化
            sword.tag = "Sword";
            anim.SetTrigger("Atack");
        }
    }
    //InputAction/右クリック
    public void OnFire2(InputAction.CallbackContext context)
    {
        //--攻撃2
        //二重読み込み防止
        if (context.performed)
        {
            //攻撃中は移動できない
            playerCanMove = false;
            //剣のタグを有効化
            sword.tag = "Sword";
            anim.SetTrigger("Atack2");
        }
    }
    //InputAction/Ctrl
    public void OnGuard(InputAction.CallbackContext context)
    {
        //--ガード
        if (context.performed)
        {
            isPlayerGeard = true;
            //ガード中は動けない
            playerCanMove = false;
            anim.SetBool("Guard",true);
        }
        //ガード解除
        if(context.canceled)
        {
            isPlayerGeard = false;
            anim.SetBool("Guard", false);
        }
    }
    //InputAction/C
    public void OnMagic(InputAction.CallbackContext context)
    {
        //特に意味のないモーション
        if (context.performed)
        {
            playerCanMove = false;
            anim.SetTrigger("Magic");
        }
    }
    //プレイヤーを移動可能状態に
    public void PlayerCanMove()
    {
        playerCanMove = true;
    }
    //プレイヤーの攻撃力を返す
    public int PlayerAtk()
    {
        return PlayerManager.Instance.PlayerAtk();
    }
    //プレイヤー死亡
    public void PlayerDeath()
    {
        playerCanMove = false;
        anim.SetTrigger("Death");
        //シーンリセット準備
        GameManager.Instance.SceneResetGameOver();
    }
    //被ダメのクールタイム
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        playerDodge = false;
    }
    //被ダメ検知
    private void OnTriggerEnter(Collider collision)
    {
        //EnemyAttackTagだった場合
        if (collision.CompareTag("EnemyAttack") && !playerDodge && !isPlayerGeard)
        {
            //プレイヤーを無敵状態へ
            playerDodge = true;
            anim.SetTrigger("Damaged");
            //被ダメのクールタイム処理
            StartCoroutine(DamagedCoolTime());
            //0以下になった場合
            if(PlayerManager.Instance.PlayerDamaged(GameManager.Instance.status.charaList[collision.transform.root.GetComponent<EnemyContoroller>().EnemyNumber()].Atk) <= 0)
            {
                //死亡
                PlayerDeath();
            }
        }
    }
}
