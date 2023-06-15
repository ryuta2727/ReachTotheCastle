using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    //プレイヤーが索敵範囲に入った時
    public void InSearchRocation()
    {
        
    }
    //プレイヤーが索敵範囲から出た時
    public void OutSearchRocation()
    {

    }
    //被ダメ後のエネミーのHPを返す
    public int EnemyDamaged(int atk,int nowHp)
    {
        Debug.Log("aaa");
        var resultHp = nowHp - atk;
        return resultHp;
    }
    //死亡時処理
    public void EnemyDeath(int enemyNum)
    {
        GameManager.Instance.PlayerGetExp(enemyNum);
        if(enemyNum == 2)
        {
            StartCoroutine(SceneReset());
        }
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
    //(引数)番のキャラクターのhpの取得
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
