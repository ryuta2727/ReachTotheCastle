using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraContorol : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���z���̃J����")]
    private CinemachineVirtualCamera vCamera2;
    //�؂�ւ����Ɍ��̃v���C�I���e�B��ێ�
    private int defaultPriorityVC2;

    // Start is called before the first frame update
    void Start()
    {
        defaultPriorityVC2 = vCamera2.Priority;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //���z���J�����ɕύX
    public void ToShoulderCameraChange()
    {
        vCamera2.Priority = 20;
    }
    //���C���J�����ɕύX *��������L�q�K�{
    public void ToMainCameraChange()
    {
        vCamera2.Priority = defaultPriorityVC2;
    }
    public void OnRight(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ToShoulderCameraChange();
        }
    }
    public void OnRightLelease(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            ToMainCameraChange();
        }
    }
}
