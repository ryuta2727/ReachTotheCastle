using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraContorol : MonoBehaviour
{
    [SerializeField]
    [Tooltip("タイトルカメラ")]
    private CinemachineVirtualCamera vCamera2;
    [SerializeField]
    PlayerInput playerInput;
    [SerializeField]
    GameObject titleCanvas;

    private bool onceTime = true;

    private void Awake()
    {
    }
    //メインのカメラに変更
    public void ToMainCameraChange()
    {

        vCamera2.Priority = -10;
    }
    public void OnRight(InputAction.CallbackContext context)
    {
        if(onceTime&& context.performed)
        {
            titleCanvas.SetActive(false);
            onceTime = false;
            ToMainCameraChange();
            StartCoroutine(CameraChangeTime());
        }
    }
    IEnumerator CameraChangeTime()
    {
        yield return new WaitForSeconds(2.2f);
        playerInput.currentActionMap = playerInput.actions.actionMaps[0];
    }
}
