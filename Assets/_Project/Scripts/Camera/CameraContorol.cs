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

    //メインのカメラに変更
    public void ToMainCameraChange()
    {
        vCamera2.Priority = -10;
    }
    //タイトル画面時の右クリック
    public void OnRight(InputAction.CallbackContext context)
    {
        if(onceTime&& context.performed)
        {
            //複数回読み込み禁止
            onceTime = false;
            //タイトルキャンバスを消す
            titleCanvas.SetActive(false);
            //メインでプレイするカメラへ遷移
            ToMainCameraChange();
            //プレイヤーの操作を有効化
            StartCoroutine(CameraChangeTime());
        }
    }
    //カメラ遷移が終わってから操作が効くように
    IEnumerator CameraChangeTime()
    {
        yield return new WaitForSeconds(2.2f);
        //InputSystemMapをプレイヤーのものへ
        playerInput.currentActionMap = playerInput.actions.actionMaps[0];
    }
}
