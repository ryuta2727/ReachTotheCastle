using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    //�v���C���[�����G�͈͂ɓ�������
    public void InSearchRocation()
    {
        
    }
    //�v���C���[�����G�͈͂���o����
    public void OutSearchRocation()
    {

    }
    //��_������
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
            StartCoroutine(SceneReset());
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
    IEnumerator SceneReset()
    {
        GameManager.Instance.GameClear();
        yield return new WaitForSeconds(8f);
        GameManager.Instance.ResetStatus();
        GameManager.Instance.SceneReset();
    }
}
