using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Cursor.visible = false;
        ResetStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerGetExp(int enemyNum)
    {
        Debug.Log("exp‚°‚Á‚Æ");
        status.charaList[0].Exp += status.charaList[enemyNum].Exp;
        if(status.charaList[0].Exp >= 10)
        {
            status.charaList[0].Lev++;
            PlayerManager.Instance.PlayerLevUp();
        }
    }
    public void ResetStatus()
    {
        status.charaList[0].Lev = 1;
        status.charaList[0].Exp = 0;
    }
    public void SceneReset()
    {
        ResetStatus();
        PlayerManager.Instance.HpReset();
        clear.SetActive(false);
        die.SetActive(false);
        SceneManager.LoadScene("3DMainScene");
    }
    public void PlayerDie()
    {
        die.SetActive(true);
    }
    public void GameClear()
    {
        clear.SetActive(true);
    }
}
