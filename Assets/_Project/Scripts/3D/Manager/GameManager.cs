using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public CharaDataBase status;
    [SerializeField]
    GameObject obj;
    [SerializeField]
    GameObject die;
    [SerializeField]
    GameObject clear;
    void Start()
    {
        //カーソル非表示
        Cursor.visible = false;
        //ステータス初期化
        ResetStatus();
    }
    //プレイヤーへ経験値を与える
    public void PlayerGetExp(int enemyNum)
    {
        status.charaList[0].Exp += status.charaList[enemyNum].Exp;
        //レベルアップ
        if(status.charaList[0].Exp >= 10)
        {
            status.charaList[0].Lev++;
            PlayerManager.Instance.PlayerLevUp();
        }
    }
    //プレイヤーのステータスレセット
    public void ResetStatus()
    {
        status.charaList[0].Lev = 1;
        status.charaList[0].Exp = 0;
    }
    //シーンのリセット
    public void SceneReset()
    {
        //ステータス初期化
        ResetStatus();
        PlayerManager.Instance.HpReset();
        //ラスト演出を消す
        clear.SetActive(false);
        die.SetActive(false);
        //シーンリセット
        SceneManager.LoadScene("3DMainScene");
    }
    //プレイヤーの死亡
    public void PlayerDie()
    {
        die.SetActive(true);
    }
    //ゲームをクリアした場合に呼ぶ
    public void SceneResetGameClear()
    {
        GameClear();
        StartCoroutine(WaitSceneReset());
    }
    //死んだ時に呼ぶ
    public void SceneResetGameOver()
    {
        PlayerDie();
        StartCoroutine(WaitSceneReset());
    }
    //ゲームクリアの演出表示
    public void GameClear()
    {
        clear.SetActive(true);
    }
    //Escキーでアプリケーション終わらす
    public void OnEscKey(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
    //シーンリセットまでの時間
    IEnumerator WaitSceneReset()
    {
        yield return new WaitForSeconds(8f);
        ResetStatus();
        SceneReset();
    }
}
