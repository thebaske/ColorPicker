using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WriteUI_INT)), CanEditMultipleObjects]
public class WriteUI_Int_Editor : Editor
{
    SerializedProperty m_usePrefix;
    SerializedProperty m_useSuffix;
    SerializedProperty m_prefix;
    SerializedProperty m_suFix;
    SerializedProperty m_autoSubscribe;
    SerializedProperty m_animateValueChage;
    SerializedProperty m_intValue;
    SerializedProperty m_OnValueUpdated;

    private void OnEnable()
    {
        m_animateValueChage = serializedObject.FindProperty("animateValueChage");
        m_intValue = serializedObject.FindProperty("intValue");
        m_usePrefix = serializedObject.FindProperty("usePrefix");
        m_useSuffix = serializedObject.FindProperty("useSufix");
        m_prefix = serializedObject.FindProperty("prefix");
        m_suFix = serializedObject.FindProperty("suFix");
        m_autoSubscribe = serializedObject.FindProperty("autSubscribe");
        m_OnValueUpdated = serializedObject.FindProperty("OnValueChanged");

    }
    public override void OnInspectorGUI()
    {
        WriteUI_INT tmp = target as WriteUI_INT;

        EditorGUILayout.PropertyField(m_autoSubscribe, new GUIContent("Auto Subscribe"));
        EditorGUILayout.PropertyField(m_OnValueUpdated, new GUIContent("OnValueChanged"));

        EditorGUILayout.PropertyField(m_animateValueChage, new GUIContent("Animate value"));
        EditorGUILayout.PropertyField(m_usePrefix, new GUIContent("Use prefix"));
        EditorGUILayout.PropertyField(m_useSuffix, new GUIContent("Use sufix"));
        // EditorGUILayout.PropertyField(m_mainValue, new GUIContent("Asset refMainValue"));


        //strings to be enabled

        Rect texPos = new Rect(250, 40, 25, 25);

        EditorGUILayout.PropertyField(m_intValue, new GUIContent("Int value"));
        if (tmp.usePrefix)
        {
            EditorGUILayout.PropertyField(m_prefix, new GUIContent("Prefix"));
        }
        if (tmp.useSufix)
        {
            EditorGUILayout.PropertyField(m_suFix, new GUIContent("Sufix"));
        }

        serializedObject.ApplyModifiedProperties();
    }


}
