using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private int playerMaxHp;  //現在レベルの最大HP
    private int playerNowHp;  //現在のHP
    private int playerAtk;
    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp;
        playerNowHp = playerMaxHp;
        playerAtk = GameManager.Instance.status.charaList[0].Atk;
    }
    //プレイヤーの被ダメ処理
    public int PlayerDamaged(int atk)
    {
        playerNowHp = playerNowHp - atk;
        //プレイヤーのHPが3より低くなったら彩度を下げる演出
        if(playerNowHp < 3)
        {
            PostCameraManager.Instance.LowHp();
        }

        return playerNowHp;
    }
    //プレイヤーの攻撃力を返す
    public int PlayerAtk()
    {
        return playerAtk;
    }
    //プレイヤーのレベルアップ処理
    public void PlayerLevUp()
    {
        GameManager.Instance.status.charaList[0].Exp = 0;
        //HPと攻撃力の上昇
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp  + GameManager.Instance.status.charaList[0].Lev * 2;
        playerAtk   = GameManager.Instance.status.charaList[0].Atk + GameManager.Instance.status.charaList[0].Lev * 1;
        //Hp回復
        playerNowHp = playerMaxHp;
        //HP低下演出が入ってたら元に戻す
        PostCameraManager.Instance.HighHp();
    }
    //Hpの初期化
    public void HpReset()
    {
        playerMaxHp = 5;
        playerNowHp = 5;
    }
}
