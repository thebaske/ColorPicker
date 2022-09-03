using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using UnityEditor.SceneManagement;
using System.Reflection;
using System;
public class ConfigWIndowOppener : MonoBehaviour
{
    //public static List<MemberInfo> exposedMembers = new List<MemberInfo>();
    
    //[MenuItem("Config/Open ConfigWindow %c")]
    //public static void OpenConfigWindow()
    //{
    //    EditorWindow.GetWindow(typeof(ConfigWindow));
    //}
    //public void OnEnable()
    //{
    //    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
    //    foreach (Assembly assembly in assemblies)
    //    {
    //        Type[] types = assembly.GetTypes();
    //        foreach (Type type in types)
    //        {
    //            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    //            MemberInfo[] members = type.GetMembers(flags);
    //            foreach (MemberInfo member in members)
    //            {
    //                ConfigAttribute configatr = member.GetCustomAttribute<ConfigAttribute>();
    //                if (configatr != null)
    //                {
    //                    exposedMembers.Add(member);
    //                }
    //            }
    //        }
    //    }
    //}
    //public void OnGUI()
    //{
    //    EditorGUILayout.LabelField("Config", EditorStyles.boldLabel);
    //    foreach (MemberInfo member in exposedMembers)
    //    {
    //        EditorGUILayout.LabelField($"{member.Name}  {member.ReflectedType}");
    //    }
    //}
}
