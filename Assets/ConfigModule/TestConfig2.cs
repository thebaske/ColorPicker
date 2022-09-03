using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConfig2 : MonoBehaviour
{
    [Config("test3", "helloVector3")]public Vector3 vcTor;
    [Config("test3", "helloString")]public string word;
    [Config("test2", "helloFloat")]public float numberWithDot;
    [Config("boolTest", "MeBool")]public bool checkBox;

    [ContextMenu("TestVectorConversion")]
    void TestVector3()
    {
        object tmp = vcTor;
        Debug.Log(tmp);
        var object__ = (Vector3)tmp;
    }
}
