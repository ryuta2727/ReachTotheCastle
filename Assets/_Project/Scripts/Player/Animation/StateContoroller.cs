using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContoroller : StateMachineBehaviour
{
    //����A�j���[�V�����X�e�[�g�ɓ��������̏���
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //locomotion��������
        if (stateInfo.IsName("locomotion"))
        {
            //Player�𓮂���悤�ɂ���
            PlayerContorol playerContorol = GameObject.Find("Player").GetComponent<PlayerContorol>();
            playerContorol.PlayerCanMove();
        }
    }
    //����A�j���[�V�����X�e�[�g����o�����̏���
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Atack1�܂���Atack2��������
        if(stateInfo.IsName("Atack1") || stateInfo.IsName("Atack2"))
        {
            //���̃^�O�𖳌���
            GameObject.Find("MoonSword").tag = "Untagged";
        }
    }
}
