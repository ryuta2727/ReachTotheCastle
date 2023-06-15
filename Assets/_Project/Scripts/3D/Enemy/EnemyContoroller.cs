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
    //徘徊時の次の目的地を設定(ランダム)
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
                break;
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
        if (enemyNumber == 1)
        {
            navMeshAgent.stoppingDistance = 3.2f;
        }
        else if(enemyNumber == 2)
        {
            navMeshAgent.stoppingDistance = 1.5f;
        }
        //TargetSetPlayer();
    }
    //プレイヤーの追従解除
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        navMeshAgent.stoppingDistance = 0;
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
        AiMainRoutine();

        aiState = nextState;

        StartCoroutine(AiTimer());
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
            else
            {
                nextState = EnemyAiState.CHASE;
            }
            
        }
        //徘徊時
        else if(isEnemyMove)
        {
            nextState = EnemyAiState.MOVE;
        }

        //死亡時
        if(isEnemyDie)
        {
            nextState = EnemyAiState.Die;
        }
    }
    #endregion
    #region 各ステートの処理
    //Idle(基本使わない)
    private void Wait()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }
    //自由移動
    private void Move()
    {
        if (!isPlayerChase && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("Walk", true);
            TargetSetRndRocation();
        }
    }
    //プレイヤーを追跡
    private void Chase()
    {
        TargetSetPlayer();
    }
    //攻撃
    private void Attack()
    {
        if (enemyCanAttack)
        {
            enemyCanAttack = false;
            //二種の攻撃からランダム
            var rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                atackcollider[0].tag = "EnemyAttack";
                anim.SetTrigger("Atack1");
                StartCoroutine(AtkTagSet(1f));  //引数は攻撃判定が残る時間
            }
            else if(rnd == 1)
            {
                atackcollider[1].tag = "EnemyAttack";
                anim.SetTrigger("Atack2");
                StartCoroutine(AtkTagSet(1f));  //引数は攻撃判定が残る時間
            }
        }
    }
    //死亡
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
    //フラグのリセット
    private void FlagReset()
    {
        isStateRunning = false;

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
    //被ダメのクールタイム
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        enemyDodge = false;
    }
    //攻撃判定の発生とクールタイム
    IEnumerator AtkTagSet(float num)
    {
        yield return new WaitForSeconds(num);  //攻撃判定が残る時間
        atackcollider[0].tag = "Untagged";
        atackcollider[1].tag = "Untagged";
        yield return new WaitForSeconds(3f);  //次に攻撃ができるようになるクールタイム
        enemyCanAttack = true;
    }
    //エネミーの被ダメ
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !enemyDodge)
        {
            enemyDodge = true;
            StartCoroutine(DamagedCoolTime());
            anim.SetTrigger("Damaged");
            //被ダメ結果後のHp
            enemyHp = EnemyManager.Instance.EnemyDamaged(collision.transform.root.gameObject.GetComponent<PlayerContorol>().PlayerAtk(), enemyHp);
            if(enemyHp <= 0)
            {
                //--フラグを初期化して死亡フラグを発生--
                FlagReset();
                isEnemyDie = true;
            }
        }
    }
    //自身のエネミー番号(種類)を返す
    public int EnemyNumber()
    {
        return enemyNumber;
    }
}
    
