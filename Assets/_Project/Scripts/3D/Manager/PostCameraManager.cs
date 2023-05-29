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

    public void LowHp()
    {
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(-80);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
    public void HighHp()
    {
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(0);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
}
