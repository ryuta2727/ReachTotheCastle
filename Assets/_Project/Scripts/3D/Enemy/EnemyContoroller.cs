using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContoroller : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    NavMeshPath path;
    Animator anim;
    [SerializeField]
    private Transform player;    //プレイヤーをセット
    [SerializeField]
    private int enemyNumber;    //このエネミーの番号
    [SerializeField]
    CapsuleCollider searchCoolider;
    [SerializeField]
    private float attackRange = 2;
    [SerializeField]
    List<GameObject> atackcollider = new List<GameObject>();

    private Vector3 nextTarget;


    //状態管理用
    private bool isStateRunning = false;
    private bool enemyWait = false;
    private bool enemyCanAttack = true;
    private bool enemyDodge = false;
    private bool isEnemyMove = true;
    private bool isPlayerChase = false;
    private bool isEnemyDie = false;
    private bool isExp = true;
    //ステータス保管
    private int enemyHp;
    private int enemyAtk;

    public enum EnemyAiState
    {
        WAIT,             //行動を停止
        MOVE,             //徘徊
        CHASE,            //追従
        ATTACK,           //停止して攻撃
        IDLE,             //待機
        AVOID,            //回避
        Die,              //死亡
    }
    public EnemyAiState aiState = EnemyAiState.WAIT;
    private EnemyAiState nextState;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        path = new NavMeshPath();
    }
    private void Start()
    {
        enemyHp = GameManager.Instance.status.charaList[enemyNumber].Hp;
        enemyAtk = GameManager.Instance.status.charaList[enemyNumber].Atk;
    }
    private void Update()
    {
        //現在のステート管理
        UpdateAi();
    }
    //追従先をプレイヤーに変更
    private void TargetSetPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
    //徘徊時の次の目的地を設定
    private void TargetSetRndRocation()
    {
        var safe = 0;
        //到達不可な場所が選定された場合やり直す
        do
        {
            #region 無限ループ保険
            safe++;
            if(safe > 100)
            {
                enemyWait = true;
            }
            #endregion
            nextTarget = new Vector3(Random.Range(-EnemyManager.Instance.EnemyMoveRange(enemyNumber), EnemyManager.Instance.EnemyMoveRange(enemyNumber)), 0,
                                     Random.Range(-EnemyManager.Instance.EnemyMoveRange(enemyNumber), EnemyManager.Instance.EnemyMoveRange(enemyNumber)));
            NavMesh.CalculatePath(transform.position, transform.position + nextTarget, NavMesh.AllAreas, path);
        } while ((path.status + "") != "PathComplete");
        navMeshAgent.SetDestination(transform.position + nextTarget);
    }
    //プレイヤーを追従
    public void ChasePlayer()
    {
        isPlayerChase = true;
        //TargetSetPlayer();
    }
    //プレイヤーの追従解除
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        //TargetSetRndRocation();
    }
    //敵AIの行動処理
    private void UpdateAi()
    {
        SetAi();

        switch (aiState)
        {
            #region 各ステートに遷移
            case EnemyAiState.WAIT:
                Wait();
                break;
            case EnemyAiState.MOVE:
                Move();
                break;
            case EnemyAiState.CHASE:
                Chase();
                break;
            case EnemyAiState.ATTACK:
                Attack();
                break;
            case EnemyAiState.IDLE:
                Idle();
                break;
            case EnemyAiState.AVOID:
                Avoid();
                break;
            case EnemyAiState.Die:
                Die();
                break;
            #endregion
        }
    }
    //ステートセット
    private void SetAi()
    {
        if (isStateRunning)
        {
            return;
        }

        InitAi();
        AiMainRoutine();

        aiState = nextState;

        StartCoroutine(AiTimer());
    }
    //初期化
    private void InitAi()
    {
        //初期化
    }
    #region メインのルーチン(ステートのセット)
    private void AiMainRoutine()
    {
        //強制停止用
        if (enemyWait)
        {
            nextState = EnemyAiState.WAIT;
            enemyWait = false;
            return;
        }
        //--ステート遷移--
        //プレイヤー追跡時
        if (isPlayerChase)
        {
            //Debug.Log(Vector3.Distance(player.position, this.transform.position));
            //攻撃
            if (enemyCanAttack && Vector3.Distance(player.position,this.transform.position) < attackRange)
            {
                nextState = EnemyAiState.ATTACK;
            }
            //追跡移動
            else if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                nextState = EnemyAiState.CHASE;
            }
            else
            {
                nextState = EnemyAiState.IDLE;
            }
            
        }
        //徘徊時
        else if(isEnemyMove)
        {
            nextState = EnemyAiState.MOVE;
        }
        //死亡時
        else if(isEnemyDie)
        {
            nextState = EnemyAiState.Die;
        }
        //
        else
        {
            nextState = EnemyAiState.IDLE;
        }
    }
    #endregion
    #region 各ステートの処理
    private void Wait()
    {

    }
    private void Move()
    {
        if (!isPlayerChase && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("Walk", true);
            TargetSetRndRocation();
        }
    }
    private void Chase()
    {
        TargetSetPlayer();
    }
    private void Attack()
    {
        if (enemyCanAttack)
        {
            enemyCanAttack = false;
            var rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                atackcollider[0].tag = "EnemyAttack";
                anim.SetTrigger("Atack1");  //60
                StartCoroutine(AtkTagSet(1f));
            }
            else if(rnd == 1)
            {
                atackcollider[1].tag = "EnemyAttack";
                anim.SetTrigger("Atack2");  //40
                StartCoroutine(AtkTagSet(1f));
            }
        }
    }
    
    private void Idle()
    {

    }
    private void Avoid()
    {

    }
    private void Die()
    {
        if (isExp)
        {
            isExp = false;
            anim.SetBool("Death", true);
            EnemyManager.Instance.EnemyDeath(enemyNumber);
        }
    }
    #endregion
    public int EnemyAtk()
    {
        return enemyAtk;
    }
    private void FlagReset()
    {
        isStateRunning = false;
        enemyWait = false;
        enemyCanAttack = false;
        isEnemyMove = false;
        isPlayerChase = false;
        isEnemyDie = false;
}
    //思考のインターバル
    IEnumerator AiTimer()
    {
        isStateRunning = true;
        yield return new WaitForSeconds(0.5f);
        isStateRunning = false;
    }
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        enemyDodge = false;
    }
    IEnumerator AtkTagSet(float num)
    {
        yield return new WaitForSeconds(num);
        atackcollider[0].tag = "Untagged";
        atackcollider[1].tag = "Untagged";
        yield return new WaitForSeconds(3f);
        enemyCanAttack = true;
    }
    IEnumerator SceneReset()
    {
        GameManager.Instance.GameClear();
        yield return new WaitForSeconds(6f);
        GameManager.Instance.ResetStatus();
        GameManager.Instance.SceneReset();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !enemyDodge)
        {
            enemyDodge = true;
            StartCoroutine(DamagedCoolTime());
            anim.SetTrigger("Damaged");
            enemyHp = EnemyManager.Instance.EnemyDamaged(collision.transform.root.gameObject.GetComponent<PlayerContorol>().PlayerAtk(), enemyHp);
            if(enemyHp < 0)
            {
                FlagReset();
                isEnemyDie = true;
            }
        }
    }
    public int EnemyNumber()
    {
        return enemyNumber;
    }
}
    
