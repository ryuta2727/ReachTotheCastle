using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    //��_����̃G�l�~�[��HP��Ԃ�
    public int EnemyDamaged(int atk,int nowHp)
    {
        Debug.Log("aaa");
        var resultHp = nowHp - atk;
        return resultHp;
    }
    //���S������
    public void EnemyDeath(int enemyNum)
    {
        GameManager.Instance.PlayerGetExp(enemyNum);
        if(enemyNum == 2)
        {
            GameManager.Instance.SceneResetGameClear();
        }
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
    //(����)�Ԃ̃L�����N�^�[��hp�̎擾
    public int EnemyAtk(int charanum)
    {
        return GameManager.Instance.status.charaList[charanum].Hp;
    }
}
