using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraContorol : MonoBehaviour
{
    [SerializeField]
    [Tooltip("肩越しのカメラ")]
    private CinemachineVirtualCamera vCamera2;
    //切り替え時に元のプライオリティを保持
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
    //肩越しカメラに変更
    public void ToShoulderCameraChange()
    {
        vCamera2.Priority = 20;
    }
    //メインカメラに変更 *増えたら記述必須
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
