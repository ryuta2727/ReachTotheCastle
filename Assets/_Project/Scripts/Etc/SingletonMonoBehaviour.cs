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
                    Debug.LogError(t + " ���A�^�b�`���Ă���GameObject�͂���܂���");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��邩���ׂ�
        // �A�^�b�`����Ă���ꍇ�͔j������B
        CheckInstance();
    }

    protected void CheckInstance()
    {
        Debug.Log("�`�F�b�N");
        if (instance == null)
        {
            Debug.Log("���v");
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("�d��");
            Destroy(this);
        }
    }
}
