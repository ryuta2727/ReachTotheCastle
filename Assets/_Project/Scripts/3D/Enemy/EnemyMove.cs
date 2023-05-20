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
    private Transform player;    //�v���C���[���Z�b�g
    [SerializeField]
    private int EnemyNumber;    //�e�G�l�~�[�̔ԍ�
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
        WAIT,             //�s�����~
        MOVE,             //�p�j
        CHASE,            //�Ǐ]
        ATTACK,           //��~���čU��
        IDLE,             //�ҋ@
        AVOID,            //���
        Die,              //���S
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
        //���݂̃X�e�[�g�Ǘ�
        UpdateAi();
    }
    //�Ǐ]����v���C���[�ɕύX
    private void TargetSetPlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }
    //�p�j���̎��̖ړI�n��ݒ�
    private void TargetSetRndRocation()
    {
        var safe = 0;
        //���B�s�ȏꏊ���I�肳�ꂽ�ꍇ��蒼��
        do
        {
            #region �������[�v�ی�
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
    //�v���C���[��Ǐ]
    public void ChasePlayer()
    {
        isPlayerChase = true;
        TargetSetPlayer();
    }
    //�v���C���[�̒Ǐ]����
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        TargetSetRndRocation();
    }
    //�GAI�̍s������
    private void UpdateAi()
    {
        SetAi();

        switch (aiState)
        {
            #region �e�X�e�[�g�ɑJ��
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
    //�X�e�[�g�Z�b�g
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
    //������
    private void InitAi()
    {
        //������
    }
    //���C���̃��[�`��(�X�e�[�g�̃Z�b�g)
    private void AiMainRoutine()
    {
        //������~�p
        if (enemyWait)
        {
            nextState = EnemyAiState.WAIT;
            enemyWait = false;
            return;
        }
        //--�X�e�[�g�J��--
        //�v���C���[�ǐՎ�
        if (isPlayerChase)
        {
            //�U��
            if (enemyCanAttack && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                nextState = EnemyAiState.ATTACK;
            }
            //�ǐՈړ�
            else if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                nextState = EnemyAiState.CHASE;
            }
            else
            {
                nextState = EnemyAiState.IDLE;
            }
            
        }
        //�p�j��
        else if(isEnemyMove)
        {
            nextState = EnemyAiState.MOVE;
        }
        //���S��
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
    #region �e�X�e�[�g�̏���
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
    //�v�l�̃C���^�[�o��
    IEnumerator AiTimer()
    {
        isStateRunning = true;
        yield return new WaitForSeconds(0.5f);
        isStateRunning = false;
    }
}
    
