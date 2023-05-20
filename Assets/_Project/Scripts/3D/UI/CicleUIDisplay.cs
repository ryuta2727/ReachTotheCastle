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

    private bool checkRightClick = true;  //RightClick���󂯕t���邩
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
        //������Ă���Ƃ����@UI�\��
        //������Ă��Ȃ��Ƃ���\��
        if(Mouse.current.leftButton.isPressed)
        {
            //Debug.Log("������Ă�");
            img.SetActive(true);
            mouseNowPosition = Input.mousePosition;
            signedAngle = Vector3.SignedAngle(Vector3.up, mouseNowPosition - mouseClickPosition, Vector3.back);
            //360�x�ɕϊ�
            //signedAngle = signedAngle < 0 ? 360 - Mathf.Abs(signedAngle) : signedAngle;
            //Debug.Log(signedAngle);
            if(signedAngle <= 36f && -36f < signedAngle)  //��ԏ�
            {

            }
            else if(signedAngle <= 108f && 36f < signedAngle)  //��Ԗ�
            {

            }
            else if(signedAngle <= 180f && 108f < signedAngle)  //�O�Ԗ�
            {

            }
            else if(signedAngle <= -108f && -180 < signedAngle)  //�l�Ԗ�
            {

            }
            else if(signedAngle <= -36f && -108 < signedAngle)  //�ܔԖ�
            {

            }
        }
        else
        {
            //Debug.Log("���ꂽ");
            img.SetActive(false);
        }
    }
}
