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
    //�p�j���̎��̖ړI�n��ݒ�(�����_��)
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
                break;
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
    //�v���C���[�̒Ǐ]����
    public void StopChasePlayer()
    {
        isPlayerChase = false;
        navMeshAgent.stoppingDistance = 0;
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
        AiMainRoutine();

        aiState = nextState;

        StartCoroutine(AiTimer());
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
            else
            {
                nextState = EnemyAiState.CHASE;
            }
            
        }
        //�p�j��
        else if(isEnemyMove)
        {
            nextState = EnemyAiState.MOVE;
        }

        //���S��
        if(isEnemyDie)
        {
            nextState = EnemyAiState.Die;
        }
    }
    #endregion
    #region �e�X�e�[�g�̏���
    //Idle(��{�g��Ȃ�)
    private void Wait()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", true);
    }
    //���R�ړ�
    private void Move()
    {
        if (!isPlayerChase && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            anim.SetBool("Walk", true);
            TargetSetRndRocation();
        }
    }
    //�v���C���[��ǐ�
    private void Chase()
    {
        TargetSetPlayer();
    }
    //�U��
    private void Attack()
    {
        if (enemyCanAttack)
        {
            enemyCanAttack = false;
            //���̍U�����烉���_��
            var rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                atackcollider[0].tag = "EnemyAttack";
                anim.SetTrigger("Atack1");
                StartCoroutine(AtkTagSet(1f));  //�����͍U�����肪�c�鎞��
            }
            else if(rnd == 1)
            {
                atackcollider[1].tag = "EnemyAttack";
                anim.SetTrigger("Atack2");
                StartCoroutine(AtkTagSet(1f));  //�����͍U�����肪�c�鎞��
            }
        }
    }
    //���S
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
    //�t���O�̃��Z�b�g
    private void FlagReset()
    {
        isStateRunning = false;

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
    //��_���̃N�[���^�C��
    IEnumerator DamagedCoolTime()
    {
        yield return new WaitForSeconds(2f);
        enemyDodge = false;
    }
    //�U������̔����ƃN�[���^�C��
    IEnumerator AtkTagSet(float num)
    {
        yield return new WaitForSeconds(num);  //�U�����肪�c�鎞��
        atackcollider[0].tag = "Untagged";
        atackcollider[1].tag = "Untagged";
        yield return new WaitForSeconds(3f);  //���ɍU�����ł���悤�ɂȂ�N�[���^�C��
        enemyCanAttack = true;
    }
    //�G�l�~�[�̔�_��
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !enemyDodge)
        {
            enemyDodge = true;
            StartCoroutine(DamagedCoolTime());
            anim.SetTrigger("Damaged");
            //��_�����ʌ��Hp
            enemyHp = EnemyManager.Instance.EnemyDamaged(collision.transform.root.gameObject.GetComponent<PlayerContorol>().PlayerAtk(), enemyHp);
            if(enemyHp <= 0)
            {
                //--�t���O�����������Ď��S�t���O�𔭐�--
                FlagReset();
                isEnemyDie = true;
            }
        }
    }
    //���g�̃G�l�~�[�ԍ�(���)��Ԃ�
    public int EnemyNumber()
    {
        return enemyNumber;
    }
}
    
