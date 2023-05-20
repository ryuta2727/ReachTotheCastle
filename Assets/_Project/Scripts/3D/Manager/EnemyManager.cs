using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    //�v���C���[�����G�͈͂ɓ�������
    public void InSearchRocation()
    {
        transform.parent.gameObject.GetComponent<EnemyMove>().ChasePlayer();
    }
    //�v���C���[�����G�͈͂���o����
    public void OutSearchRocation()
    {

    }
    //��_������
    public int EnmyDamaged(int atk,int nowHp)
    {
        Debug.Log("aaa");
        var resultHp = nowHp - atk;
        return resultHp;
    }
    //���S������
    public void EnemyDeath()
    {
        //���S���̏���������
        Debug.Log("���S");
    }
    //(����)�Ԃ̃L�����N�^�[�̍ő�Hp�̎擾
    public int EnemyMAxHp(int charaNum)
    {
        return GameManager.Instance.status.charaList[charaNum].Hp;
    }
    //(����)�Ԃ̃L�����N�^�[�̈ړ������̎擾
    public int EnemyMoveRange(int charanum)
    {
        return GameManager.Instance.status.charaList[charanum].MoveRange;
    }
}
