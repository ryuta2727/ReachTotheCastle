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
    //HP���Ⴍ�Ȃ������̉��o
    public void LowHp()
    {
        //�ʓx��������
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(-70);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
    //HP���񕜂������̏���
    public void HighHp()
    {
        //�ʓx���t���b�g�ɖ߂�
        colorGrading.enabled.Override(true);
        colorGrading.saturation.Override(0);
        PostProcessManager.instance.QuickVolume(gameObject.layer, 0, colorGrading);
    }
}
