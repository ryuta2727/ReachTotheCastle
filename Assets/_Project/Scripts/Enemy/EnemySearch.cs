using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    [SerializeField]
    EnemyContoroller enemyContoroller;
    //プレイヤーがエネミーの検知範囲に入ってきた時
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            //プレイヤーを追跡
            enemyContoroller.ChasePlayer();
        }
    }
    //プレイヤーが検知範囲外に出た場合
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //追跡をやめる
            enemyContoroller.StopChasePlayer();
        }
    }
}
