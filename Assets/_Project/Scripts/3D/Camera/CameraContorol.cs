using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraContorol : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�^�C�g���J����")]
    private CinemachineVirtualCamera vCamera2;
    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    GameObject titleCanvas;

    private bool onceTime = true;

    //���C���̃J�����ɕύX
    public void ToMainCameraChange()
    {
        vCamera2.Priority = -10;
    }
    //�^�C�g����ʎ��̉E�N���b�N
    public void OnRight(InputAction.CallbackContext context)
    {
        if(onceTime&& context.performed)
        {
            //������ǂݍ��݋֎~
            onceTime = false;
            //�^�C�g���L�����o�X������
            titleCanvas.SetActive(false);
            //���C���Ńv���C����J�����֑J��
            ToMainCameraChange();
            //�v���C���[�̑����L����
            StartCoroutine(CameraChangeTime());
        }
    }
    //�J�����J�ڂ��I����Ă��瑀�삪�����悤��
    IEnumerator CameraChangeTime()
    {
        yield return new WaitForSeconds(2.2f);
        //InputSystemMap���v���C���[�̂��̂�
        playerInput.currentActionMap = playerInput.actions.actionMaps[0];
    }
}
