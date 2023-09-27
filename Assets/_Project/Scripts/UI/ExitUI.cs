using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ExitUI : MonoBehaviour
{
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickCloseTab()
    {
        this.gameObject.SetActive(false);
    }
    public void OnClickToTitle()
    {
        GameManager.Instance.NoWaitReset();
    }
}
