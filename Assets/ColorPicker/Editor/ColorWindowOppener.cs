using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorWindowOppener : MonoBehaviour
{
    [MenuItem("ColorPicker/Palette window %t")]
    static void DoSomethingWithAShortcutKey()
    {
       EditorWindow.GetWindow(typeof(ColorPickerEditorWindow));
    }
}
