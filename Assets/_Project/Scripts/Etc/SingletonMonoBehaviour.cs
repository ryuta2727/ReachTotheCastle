using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected void CheckInstance()
    {
        Debug.Log("チェック");
        if (instance == null)
        {
            Debug.Log("大丈夫");
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("重複");
            Destroy(this);
        }
    }
}
