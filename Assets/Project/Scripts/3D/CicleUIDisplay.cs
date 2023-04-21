using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CicleUIDisplay : MonoBehaviour
{
    [SerializeField]
    GameObject img;

    private Vector3 mouseClickPosition = Vector3.zero;
    private Vector3 mouseNowPosition = Vector3.zero;
    private float signedAngle = 0;

    private bool checkRightClick = true;  //RightClickを受け付けるか
    public void RightClick(InputAction.CallbackContext context)
    {
        if(checkRightClick && context.performed)
        {
            Debug.Log("mouse");
            mouseClickPosition = Input.mousePosition;
        }
    }
    private void Awake()
    {
        //img = GetComponent<Image>();
    }
    private void Update()
    {
        //押されているとき魔法UI表示
        //押されていないとき非表示
        if(Mouse.current.leftButton.isPressed)
        {
            //Debug.Log("押されてる");
            img.SetActive(true);
            mouseNowPosition = Input.mousePosition;
            signedAngle = Vector3.SignedAngle(Vector3.up, mouseNowPosition - mouseClickPosition, Vector3.back);
            //360度に変換
            //signedAngle = signedAngle < 0 ? 360 - Mathf.Abs(signedAngle) : signedAngle;
            //Debug.Log(signedAngle);
            if(signedAngle <= 36f && -36f < signedAngle)  //一番上
            {

            }
            else if(signedAngle <= 108f && 36f < signedAngle)  //二番目
            {

            }
            else if(signedAngle <= 180f && 108f < signedAngle)  //三番目
            {

            }
            else if(signedAngle <= -108f && -180 < signedAngle)  //四番目
            {

            }
            else if(signedAngle <= -36f && -108 < signedAngle)  //五番目
            {

            }
        }
        else
        {
            //Debug.Log("離れた");
            img.SetActive(false);
        }
    }
}
