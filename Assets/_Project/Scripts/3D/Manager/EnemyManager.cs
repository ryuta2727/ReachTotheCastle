using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    //プレイヤーが索敵範囲に入った時
    public void InSearchRocation()
    {
        transform.parent.gameObject.GetComponent<EnemyMove>().ChasePlayer();
    }
    //プレイヤーが索敵範囲から出た時
    public void OutSearchRocation()
    {

    }
    //被ダメ処理
    public int EnmyDamaged(int atk,int nowHp)
    {
        Debug.Log("aaa");
        var resultHp = nowHp - atk;
        return resultHp;
    }
    //死亡時処理
    public void EnemyDeath()
    {
        //死亡時の処理を書く
        Debug.Log("死亡");
    }
    //(引数)番のキャラクターの最大Hpの取得
    public int EnemyMAxHp(int charaNum)
    {
        return GameManager.Instance.status.charaList[charaNum].Hp;
    }
    //(引数)番のキャラクターの移動距離の取得
    public int EnemyMoveRange(int charanum)
    {
        return GameManager.Instance.status.charaList[charanum].MoveRange;
    }
}
