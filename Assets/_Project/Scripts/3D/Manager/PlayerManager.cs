using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [SerializeField]
    GameObject player;
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public int PlayerDamaged(int atk)
    {
        Debug.Log(playerNowHp);
        playerNowHp = playerNowHp - atk;
        Debug.Log(playerNowHp);
        return playerNowHp;
    }
    public void PlayerDeath()
    {

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
    }
}
