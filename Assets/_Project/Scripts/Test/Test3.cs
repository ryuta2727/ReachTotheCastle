using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    public void Test1()
    {
        Test2();
    }
    public void Test2()
    {
        this.gameObject.SetActive(false);
    }
}
