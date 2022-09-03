#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

using UnityToolbarExtender;
[InitializeOnLoad]
public static class CustomTagSelector 
{
    static string[] tags;

    static string[] scenesFullPath;

    static int selectedIndex;

    static CustomTagSelector()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        OnEnable();
    }

    static void OnEnable()
    {
        int tagsCount = UnityEditorInternal.InternalEditorUtility.tags.Length;
        tags = UnityEditorInternal.InternalEditorUtility.tags;
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();



         int tmpIndex = EditorGUILayout.Popup(selectedIndex, UnityEditorInternal.InternalEditorUtility.tags);
        if (tmpIndex != selectedIndex)
        {
            selectedIndex = tmpIndex;
            EditorGUIUtility.systemCopyBuffer = UnityEditorInternal.InternalEditorUtility.tags[selectedIndex]; 
        }
        if (GUILayout.Button("CopyTag"))
        {
            EditorGUIUtility.systemCopyBuffer = UnityEditorInternal.InternalEditorUtility.tags[selectedIndex];
        }
    }



}


#endif