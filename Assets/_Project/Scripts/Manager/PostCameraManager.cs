using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostCameraManager : SingletonMonoBehaviour<PostCameraManager>
{
    ColorGrading colorGrading;
    // Start is called before the first frame update
    void Start()
    {
        colorGrading = ScriptableObject.CreateInstance<ColorGrading>();
    }
    //HPが低くなった時の演出
    public void LowHp()
    {
        //彩度を下げる
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(-70);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
    //HPが回復した時の処理
    public void HighHp()
    {
        //彩度をフラットに戻す
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(0);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
}
