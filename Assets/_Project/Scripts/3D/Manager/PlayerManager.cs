using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private int playerMaxHp;
    private int playerNowHp;
    private int playerAtk;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp;
        playerNowHp = playerMaxHp;
        playerAtk = GameManager.Instance.status.charaList[0].Atk;
    }
    public int PlayerDamaged(int atk)
    {
        playerNowHp = playerNowHp - atk;
        if(playerNowHp < 5)
        {
            PostCameraManager.Instance.LowHp();
        }
        return playerNowHp;
    }
    public int PlayerAtk()
    {
        return playerAtk;
    }
    public void PlayerLevUp()
    {
        GameManager.Instance.status.charaList[0].Exp = 0;
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp  + GameManager.Instance.status.charaList[0].Lev * 2;
        playerNowHp = playerMaxHp;
        playerAtk   = GameManager.Instance.status.charaList[0].Atk + GameManager.Instance.status.charaList[0].Lev * 1;
        PostCameraManager.Instance.HighHp();
    }
    public void HpReset()
    {
        playerMaxHp = 10;
        playerNowHp = 10;
    }
}
