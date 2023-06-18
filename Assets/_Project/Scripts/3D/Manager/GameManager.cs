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
        //�J�[�\����\��
        Cursor.visible = false;
        //�X�e�[�^�X������
        ResetStatus();
    }
    //�v���C���[�֌o���l��^����
    public void PlayerGetExp(int enemyNum)
    {
        status.charaList[0].Exp += status.charaList[enemyNum].Exp;
        //���x���A�b�v
        if(status.charaList[0].Exp >= 10)
        {
            status.charaList[0].Lev++;
            PlayerManager.Instance.PlayerLevUp();
        }
    }
    //�v���C���[�̃X�e�[�^�X���Z�b�g
    public void ResetStatus()
    {
        status.charaList[0].Lev = 1;
        status.charaList[0].Exp = 0;
    }
    //�V�[���̃��Z�b�g
    public void SceneReset()
    {
        //�X�e�[�^�X������
        ResetStatus();
        PlayerManager.Instance.HpReset();
        //���X�g���o������
        clear.SetActive(false);
        die.SetActive(false);
        //�V�[�����Z�b�g
        SceneManager.LoadScene("3DMainScene");
    }
    //�v���C���[�̎��S
    public void PlayerDie()
    {
        die.SetActive(true);
    }
    //�Q�[�����N���A�����ꍇ�ɌĂ�
    public void SceneResetGameClear()
    {
        GameClear();
        StartCoroutine(WaitSceneReset());
    }
    //���񂾎��ɌĂ�
    public void SceneResetGameOver()
    {
        PlayerDie();
        StartCoroutine(WaitSceneReset());
    }
    //�Q�[���N���A�̉��o�\��
    public void GameClear()
    {
        clear.SetActive(true);
    }
    //Esc�L�[�ŃA�v���P�[�V�����I��炷
    public void OnEscKey(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
    //�V�[�����Z�b�g�܂ł̎���
    IEnumerator WaitSceneReset()
    {
        yield return new WaitForSeconds(8f);
        ResetStatus();
        SceneReset();
    }
}
