using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test2 : MonoBehaviour
{
	public float wanderRange;
	private NavMeshAgent navMeshAgent;
	private NavMeshHit navMeshHit;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.destination = transform.position;
		SetDestination();
		navMeshAgent.avoidancePriority = Random.Range(0, 100);
	}

	void RandomWander()
	{
		//�w�肵���ړI�n�ɏ�Q�������邩�ǂ����A�����������B�\�Ȃ̂����m�F���Ė��Ȃ���΃Z�b�g����B
		//pathPending �o�H�T���̏����ł��Ă��邩�ǂ���
		if (!navMeshAgent.pathPending)
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				//hasPath �G�[�W�F���g���o�H�������Ă��邩�ǂ���
				//navMeshAgent.velocity.sqrMagnitude�̓X�s�[�h
				if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
				{
					SetDestination();
				}
			}
		}
	}

	void SetDestination()
	{
		Vector3 randomPos = new Vector3(Random.Range(-wanderRange, wanderRange), 0, Random.Range(-wanderRange, wanderRange));
		//SamplePosition�͐ݒ肵���ꏊ����5�͈̔͂ōł��߂�������Bake���ꂽ�ꏊ��T���B
		NavMesh.SamplePosition(randomPos, out navMeshHit, 5, 1);
		navMeshAgent.destination = navMeshHit.position;
	}
}
