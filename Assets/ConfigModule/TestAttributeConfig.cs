using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TestAttributeConfig : MonoBehaviour
{
    
    [Config("Test Attribute", "custom data 1")]
    public int customTestData  = 43;
    [Config("Test Attribute", "custom data 2")]
    public int customTestData1 = 10;
    [Config("Test Attribute", "custom data 3")]
    public int customTestData13 = 15;
    

}
