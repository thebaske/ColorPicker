using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ConfigAttribute : Attribute
{
    public string category;
    public string message;
    public ConfigAttribute(string cat, string messge)
    {
        category = cat;
        message = messge;
    }
}
