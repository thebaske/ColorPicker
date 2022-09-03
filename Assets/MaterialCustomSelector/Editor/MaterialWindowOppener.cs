﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using UnityEditor.SceneManagement;
public class MaterialWindowOppener : MonoBehaviour
{
    [MenuItem("MyMenu/Materials window %g")]
    static void DoSomethingWithAShortcutKey()
    {
       EditorWindow.GetWindow(typeof(MaterialsEditorWindow));
    }
    [MenuItem("Scenes/Knife")]
    static void OpenSceneKnife()
    {
        EditorSceneManager.OpenScene("Assets/_00Scenes/_MainScene.unity", OpenSceneMode.Single);
    }
    [MenuItem("Scenes/BallzScene")]
    static void OpenSceneBallz()
    {
        EditorSceneManager.OpenScene("Assets/_00Scenes/_MainScene_Ballz.unity", OpenSceneMode.Single);
    }

}
