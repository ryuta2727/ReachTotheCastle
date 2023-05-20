using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    NavMeshPath path;
    Animator anim;
    [SerializeField]
    private Transform player;    //プレイヤーをセット
    [SerializeField]
    private int EnemyNumber;    //各エネミーの番号
    [SerializeField]
    CapsuleCollider searchCoolider;

    private Vector3 nextTarget;

    private bool isStateRunning = false;

    private bool enemyWait = false;
    private bool isEnemyMove = true;
    private bool enemyCanAttack = true;
    private bool isEnemyDie = false;
    private bool isPlayerChase = false;

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
            nextTarget = new Vector3(Random.Range(-EnemyManager.Instance.EnemyMoveRange(EnemyNumber), EnemyManager.Instance.EnemyMoveRange(EnemyNumber)), 0,
                                     Random.Range(-EnemyManager.Instance.EnemyMoveRange(EnemyNumber), EnemyManager.Instance.EnemyMoveRange(EnemyNumber)));
            NavMesh.CalculatePath(transform.position, transform.position + nextTarget, NavMesh.AllAreas, path);
        } while ((path.status + "") != "PathComplete");
        navMeshAgent.SetDestination(transform.position + nextTarget);
    }
    //プレイヤーを追従
    public void ChasePlayer()
    {
        isPlayerChase = true;
        TargetSetPlayer();
    }
    //プレイヤーの追従解除
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        TargetSetRndRocation();
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
    //メインのルーチン(ステートのセット)
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
            //攻撃
            if (enemyCanAttack && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
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

    }
    
    private void Idle()
    {

    }
    private void Avoid()
    {

    }
    private void Die()
    {

    }
    #endregion
    //思考のインターバル
    IEnumerator AiTimer()
    {
        isStateRunning = true;
        yield return new WaitForSeconds(0.5f);
        isStateRunning = false;
    }
}
    
