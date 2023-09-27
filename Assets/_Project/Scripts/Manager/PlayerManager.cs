using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private int playerMaxHp;  //���݃��x���̍ő�HP
    private int playerNowHp;  //���݂�HP
    private int playerAtk;
    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp;
        playerNowHp = playerMaxHp;
        playerAtk = GameManager.Instance.status.charaList[0].Atk;
    }
    //�v���C���[�̔�_������
    public int PlayerDamaged(int atk)
    {
        playerNowHp = playerNowHp - atk;
        //�v���C���[��HP��3���Ⴍ�Ȃ�����ʓx�������鉉�o
        if(playerNowHp < 3)
        {
            PostCameraManager.Instance.LowHp();
        }

        return playerNowHp;
    }
    //�v���C���[�̍U���͂�Ԃ�
    public int PlayerAtk()
    {
        return playerAtk;
    }
    //�v���C���[�̃��x���A�b�v����
    public void PlayerLevUp()
    {
        GameManager.Instance.status.charaList[0].Exp = 0;
        //HP�ƍU���͂̏㏸
        playerMaxHp = GameManager.Instance.status.charaList[0].Hp  + GameManager.Instance.status.charaList[0].Lev * 2;
        playerAtk   = GameManager.Instance.status.charaList[0].Atk + GameManager.Instance.status.charaList[0].Lev * 1;
        //Hp��
        playerNowHp = playerMaxHp;
        //HP�ቺ���o�������Ă��猳�ɖ߂�
        PostCameraManager.Instance.HighHp();
    }
    //Hp�̏�����
    public void HpReset()
    {
        playerMaxHp = 5;
        playerNowHp = 5;
    }
}
