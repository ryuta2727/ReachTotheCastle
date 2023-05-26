using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateContoroller : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("locomotion"))
        {
            PlayerContorol playerContorol = GameObject.Find("Player").GetComponent<PlayerContorol>();
            playerContorol.PlayerCanMove();
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.IsName("Atack1") || stateInfo.IsName("Atack2"))
        {
            GameObject.Find("MoonSword").tag = "Untagged";
        }
    }
}
