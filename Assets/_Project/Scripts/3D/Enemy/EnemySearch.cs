using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    [SerializeField]
    EnemyContoroller enemyContoroller;
    //�v���C���[���G�l�~�[�̌��m�͈͂ɓ����Ă�����
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            //�v���C���[��ǐ�
            enemyContoroller.ChasePlayer();
        }
    }
    //�v���C���[�����m�͈͊O�ɏo���ꍇ
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //�ǐՂ���߂�
            enemyContoroller.StopChasePlayer();
        }
    }
}
