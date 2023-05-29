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
    private Transform player;    //�v���C���[���Z�b�g
    [SerializeField]
    private int enemyNumber;    //���̃G�l�~�[�̔ԍ�
    [SerializeField]
    CapsuleCollider searchCoolider;
    [SerializeField]
    private float attackRange = 2;
    [SerializeField]
    List<GameObject> atackcollider = new List<GameObject>();

    private Vector3 nextTarget;


    //��ԊǗ��p
    private bool isStateRunning = false;
    private bool enemyWait = false;
    private bool enemyCanAttack = true;
    private bool enemyDodge = false;
    private bool isEnemyMove = true;
    private bool isPlayerChase = false;
    private bool isEnemyDie = false;
    private bool isExp = true;
    //�X�e�[�^�X�ۊ�
    private int enemyHp;
    private int enemyAtk;

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
    private void Start()
    {
        enemyHp = GameManager.Instance.status.charaList[enemyNumber].Hp;
        enemyAtk = GameManager.Instance.status.charaList[enemyNumber].Atk;
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
            nextTarget = new Vector3(Random.Range(-EnemyManager.Instance.EnemyMoveRange(enemyNumber), EnemyManager.Instance.EnemyMoveRange(enemyNumber)), 0,
                                     Random.Range(-EnemyManager.Instance.EnemyMoveRange(enemyNumber), EnemyManager.Instance.EnemyMoveRange(enemyNumber)));
            NavMesh.CalculatePath(transform.position, transform.position + nextTarget, NavMesh.AllAreas, path);
        } while ((path.status + "") != "PathComplete");
        navMeshAgent.SetDestination(transform.position + nextTarget);
    }
    //�v���C���[��Ǐ]
    public void ChasePlayer()
    {
        isPlayerChase = true;
        //TargetSetPlayer();
    }
    //�v���C���[�̒Ǐ]����
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        //TargetSetRndRocation();
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
    #region ���C���̃��[�`��(�X�e�[�g�̃Z�b�g)
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
            //Debug.Log(Vector3.Distance(player.position, this.transform.position));
            //�U��
            if (enemyCanAttack && Vector3.Distance(player.position,this.transform.position) < attackRange)
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
    #endregion
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
    //�v�l�̃C���^�[�o��
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
    
