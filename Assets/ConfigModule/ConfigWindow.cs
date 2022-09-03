using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;


public class ConfigWindow : EditorWindow
{
    static List<MemberInfoHolder> exposedMembers = new List<MemberInfoHolder>();
    JsonDataList allGameData = new JsonDataList();
    [MenuItem("Config/Open ConfigWindow111 %c")]
    public static void OpenConfigWindow()
    {   
        EditorWindow.GetWindow(typeof(ConfigWindow));
    }
    void PopulateMembersList()
    {
        allGameData.allGameData = new List<JsonDataHolder>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ;
                MemberInfo[] members = type.GetMembers(flags);
                foreach (MemberInfo member in members)
                {
                    if (member.CustomAttributes.Count() > 0)
                    {
                        ConfigAttribute configatr = member.GetCustomAttribute<ConfigAttribute>();
                        if (configatr != null)
                        {
                            MemberInfoHolder mih = new MemberInfoHolder();
                            mih.member = member;
                            mih.category = configatr.category;
                            mih.message = configatr.message;
                            exposedMembers.Add(mih);
                            allGameData.allGameData.Add(new JsonDataHolder("", mih.member.ReflectedType.ToString()));
                        }
                    }
                }
            }
        }
    }
    public void OnGUI()
    {
        if (GUILayout.Button("Get Configs"))
        {
            exposedMembers.Clear();
            PopulateMembersList();
        }
        if (GUILayout.Button("Save settings"))
        {
            Save();
        }
        DisplayList();
    }
    void Save()
    {
        Debug.Log($"{allGameData.allGameData.ToString()}");
        //System.IO.File.WriteAllText(Application.dataPath + "/Config/USERSDATA/" + UserBasedJsonFile, todos);
    }
    public void DisplayList()
    {
        EditorGUILayout.LabelField("Config", EditorStyles.boldLabel);
        foreach (MemberInfoHolder member in exposedMembers)
        {   
            EditorGUILayout.BeginHorizontal();
            FieldInfo fidle = (FieldInfo)member.member;
            var container2 = MonoBehaviour.FindObjectOfType(member.member.ReflectedType);
            if (fidle != null && container2 != null)
            {
                GUILayoutOption[] options = new GUILayoutOption[1];
                //var container = Activator.CreateInstance(member.ReflectedType);
                int tmp = 0;
                string str = fidle.GetValue(container2).ToString();
                var value_parsed = fidle.GetValue(container2);
               
                EditorGUILayout.LabelField($"{member.category} ");
                EditorGUILayout.LabelField(member.member.ReflectedType.ToString(), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
                EditorGUILayout.LabelField($"{member.message} ");
                fidle.SetValue(container2 ,TypeTOEditor(value_parsed));
                SetValueIntoJsonData(tmp.ToString(), member.member.ReflectedType.ToString());
                // if (Int32.TryParse(str, out tmp))
                // {
                //     EditorGUILayout.LabelField($"{member.category} ");
                //     EditorGUILayout.LabelField(member.member.ReflectedType.ToString(), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
                //     EditorGUILayout.LabelField($"{member.message} ");
                //     fidle.SetValue(container2 ,EditorGUILayout.IntField(tmp, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f)));
                //     SetValueIntoJsonData(tmp.ToString(), member.member.ReflectedType.ToString());
                // }
            }
            // else if (fidle != null && fidle.FieldType == typeof(float))
            // {   
            //     float tmpf = 0f;
            //     string str = fidle.GetValue(container2).ToString();
            //     if (float.TryParse(str, out tmpf))
            //     {
            //         EditorGUILayout.LabelField($"{member.category} ");
            //         EditorGUILayout.LabelField(member.member.ReflectedType.ToString(), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
            //         EditorGUILayout.LabelField($"{member.message} ");
            //         fidle.SetValue(container2, EditorGUILayout.FloatField(tmpf, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f)));
            //         SetValueIntoJsonData(tmpf.ToString(), member.member.ReflectedType.ToString());
            //     }
            // }
            EditorGUILayout.EndHorizontal();
        }
    }

    object TypeTOEditor(object value)
    {   
        switch (value)
        {
            case Vector3 tmp:
                value = EditorGUILayout.Vector3Field("vc3",(Vector3)value);
                break;
            case int tmp1:
               value = EditorGUILayout.IntField((int) value, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
                break;
            case string tmpstring:
                value = EditorGUILayout.TextField(value.ToString(), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
                break;
            case float tmpFloat:
                value = EditorGUILayout.FloatField((float)value, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(20f));
                break;
            case bool tmpBool:
                value = EditorGUILayout.Toggle("", (bool) value);
                break;
        }

        return value;
    }
    void SetValueIntoJsonData(string value, string name)
    {
        for (int i = 0; i < allGameData.allGameData.Count; i++)
        {
            if (allGameData.allGameData[i].valueName == name)
            {
                allGameData.allGameData[i].valueData = value;
            }
        }
    }
}
[System.Serializable]
public class MemberInfoHolder
{
    public MemberInfo member;
    public string category;
    public string message;
}
[System.Serializable]
public class JsonDataHolder
{
    public string valueData;
    public string valueName;
    public JsonDataHolder(string value, string name)
    {
        valueData = value;
        valueName = name;
    }
}
[System.Serializable]
public class JsonDataList
{
    public List<JsonDataHolder> allGameData;
}
//public struct MemberTypeHolder<T>
//{
//    UnityEngine.Object instance;
//    string value;

//}
