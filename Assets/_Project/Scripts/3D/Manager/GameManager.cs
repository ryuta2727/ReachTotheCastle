using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public CharaDataBase status;

    void Start()
    {
        Cursor.visible = false;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerGetExp(int enemyNum)
    {
        Debug.Log("exp‚°‚Á‚Æ");
        status.charaList[0].Exp += status.charaList[enemyNum].Exp;
        if(status.charaList[0].Exp > 10)
        {
            status.charaList[0].Lev++;
            PlayerManager.Instance.PlayerLevUp();
        }
    }
    public void Reset()
    {
        status.charaList[0].Lev = 1;
        status.charaList[0].Exp = 0;
    }
}
