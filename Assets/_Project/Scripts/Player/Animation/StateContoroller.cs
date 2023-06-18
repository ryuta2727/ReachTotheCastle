using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContoroller : StateMachineBehaviour
{
    //特定アニメーションステートに入った時の処理
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //locomotionだった時
        if (stateInfo.IsName("locomotion"))
        {
            //Playerを動けるようにする
            PlayerContorol playerContorol = GameObject.Find("Player").GetComponent<PlayerContorol>();
            playerContorol.PlayerCanMove();
        }
    }
    //特定アニメーションステートから出た時の処理
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Atack1またはAtack2だった時
        if(stateInfo.IsName("Atack1") || stateInfo.IsName("Atack2"))
        {
            //剣のタグを無効化
            GameObject.Find("MoonSword").tag = "Untagged";
        }
    }
}
