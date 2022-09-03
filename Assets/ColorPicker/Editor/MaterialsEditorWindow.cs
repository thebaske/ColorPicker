using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MaterialsEditorWindow : EditorWindow
{
    public MaterialsList matList;
    Vector2 scrollPos;
    public int submeshIndex;
    public int sharedMatsCount;
    GameObject currentSelected;
    private Color[] clr;
    [SerializeField]private List<TextureData> textures;
    private List<Texture2D> tex;
    private Texture2D atlas;
    private Vector2 scrolPos;
    public static void OpenMaterialsEditorWindow ()
    {
        GetWindow<MaterialsEditorWindow> ();
    }
    
    public float LerpCustom(float a, float b, float t)
    {
        return (1.0f - t) * a + b * t;
    }
    public float InverseLerpCUstom(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }
    /// <summary>
    /// Takes two input parameters(iMin, iMax) and remaps those values to 
    /// oMin and oMax by v.
    /// if v == iMin output = oMin
    /// if v == iMax output = oMax
    /// </summary>    
    public float RemapValues(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = InverseLerpCUstom(iMin, iMax, v);
        return LerpCustom(oMin, oMax, t);
    }
    
    private void OnGUI ()
    {
        if (GUILayout.Button("Reset", GUILayout.MaxWidth(70)))
        {
            textures = new List<TextureData>();
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i] = new TextureData();
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.MaxWidth(70)))
        {
            textures = new List<TextureData>();
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i] = new TextureData();
            }
        }
        EditorGUILayout.EndHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        if (textures is null || textures.Count < 9)
        {
            textures = new List<TextureData>();
            for (int i = 0; i < textures.Count; i++)
            {
                textures[i] = new TextureData();
            }
        }
        for (int i = 0; i < textures.Count; i++)
        {
            
            textures[i].color =
                EditorGUILayout.ColorField(textures[i].color, GUILayout.Width(50f), GUILayout.Height(50f));
            // textures[i].SetColor();
            GUI.backgroundColor = textures[i].color;
            if (GUILayout.Button($"Set {i}", GUILayout.MaxWidth(70)))
            {
                if (Selection.activeGameObject != null)
                {
                    MeshRenderer renderer = Selection.activeGameObject.GetComponent<MeshRenderer>();
                    // renderer.sharedMaterial.mainTexture = textures[i].tex;
                    MeshFilter mf = Selection.activeGameObject.GetComponent<MeshFilter>();
                    Vector2[] uv = new Vector2[mf.sharedMesh.uv.Length];
                    float pixelValue = RemapValues(0, textures.Count - 1, 0.01f, 0.97f, i);
                    Debug.Log($"Pixel value {pixelValue}");
                    for (int j = 0; j < uv.Length; j++)
                    {
                        uv[j] = new Vector2(pixelValue, 0f);
                    }

                    mf.sharedMesh.uv = uv;
                    renderer.sharedMaterial.mainTexture = atlas;
                    EditorUtility.SetDirty(Selection.activeGameObject);
                }
            }
        }
        if (GUILayout.Button("Pack", GUILayout.MaxWidth(70)))
        {
            PackTextures(textures.ToArray());
        }
        atlas = (Texture2D)EditorGUILayout.ObjectField(atlas, typeof(Texture2D), false, GUILayout.Width(170), GUILayout.Height(170));
        // EditorGUILayout.BeginScrollView();
        // for (int i = 0; i < clr.Length; i++)
        // {
        //     clr[i] = EditorGUILayout.ColorField(clr[i], GUILayout.Width(50f), GUILayout.Height(50f));
        //     tex[i] = new Texture2D(50, 50);
        //
        //     
        // }
        
        // for (int q = 0; q < clr.Length; q++)
        // {
        //    
        // }
        // TextureField(clr.ToString(), tex);
        EditorGUILayout.EndScrollView();
        
    }

    private void PackTextures(TextureData[] datas)
    {
        
        int atlasSizeX = datas.Length * 50;
        atlas = new Texture2D(atlasSizeX, 1);
        Color32[] atlasColors = atlas.GetPixels32();
        int colorCounter = 0;
        for (int i = 0; i < atlasColors.Length; i++)
        {
            atlasColors[i] = datas[(int)RemapValues(0, atlasColors.Length, 0, datas.Length, i)].color;
        }
        atlas.SetPixels32(atlasColors);
        // Rect[] tmp = atlas.PackTextures(texArray, 0, 1024);
        AssetDatabase.CreateAsset(atlas, "Assets/ColorPicker/ColorPicker_Atlas/" + "SharedAtlas" + ".Texture2D");
        EditorUtility.SetDirty(atlas);
    }
    private Texture2D TextureField(string name, Texture2D texture)
    {
        
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
        
        return result;
    }
    private void MatToListManagement()
    {
        GUI.backgroundColor = Color.white;
        if (GUILayout.Button("+"))
        {
            Material material = new Material(matList.allMaterials[0]);
            AssetDatabase.CreateAsset(material, "Assets/MaterialCustomSelector//BaseMaterials/MyMaterial.mat");
            matList.allMaterials.Add(material);
        }
        if (GUILayout.Button("-"))
        {
            matList.allMaterials.Remove(matList.allMaterials[matList.allMaterials.Count-1]);
        }
    }
    private void DisplayMaterialPallette()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<MeshRenderer>() != null)
        {
            if (currentSelected != Selection.activeGameObject)
            {
                submeshIndex = 0;
                currentSelected = Selection.activeGameObject;
            }

            MeshRenderer renderer = Selection.activeGameObject.GetComponent<MeshRenderer>();
            sharedMatsCount = renderer.sharedMaterials.Length;
            if (renderer.sharedMaterials.Length > 1)
            {
                string btnName = $"materials: {Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterials.Length} | edditing: {submeshIndex}";

                if (GUILayout.Button(btnName))
                {
                    submeshIndex++;
                    if (submeshIndex > Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterials.Length - 1)
                    {
                        submeshIndex = 0;
                    }
                }
            }
        }
        foreach (var item in matList.allMaterials)
        {
            if (item != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = item.color;

                if (GUILayout.Button(item.name, GUILayout.MaxWidth(70)))
                {
                    ProcessButtonClick(item);
                }
                var tmp = EditorGUILayout.ObjectField(item, typeof(Material), false, GUILayout.MaxWidth(70));
                EditorGUILayout.EndHorizontal(); 
            }
        }
    }
    private void ProcessButtonClick(Material clickedMat)
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (Selection.gameObjects[i] != null && Selection.gameObjects[i].GetComponent<MeshRenderer>() != null)
            {
                if (clickedMat.mainTexture != null)
                {
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), clickedMat.mainTexture);
                }

                MeshRenderer mshRndr = Selection.gameObjects[i].GetComponent<MeshRenderer>();
                if (sharedMatsCount > 1)
                {
                    Material[] tmp = mshRndr.sharedMaterials;
                    tmp[submeshIndex] = clickedMat;
                    mshRndr.sharedMaterials = tmp;
                }
                else
                {
                    mshRndr.sharedMaterial = clickedMat;
                }

            }
            else
            {
                Debug.Log("Select a gameobject with Mesh Renderer");
            }
        }
    }
    private void NextSubmesh ()
    {
        submeshIndex++;
    }
}
[System.Serializable]
public class TextureData
{
    public Color color;
    public Texture2D tex;

    public void SetColor()
    {
        if (tex != null)
        {
            
        }
        else
        {
            tex = new Texture2D(50, 50);
        }
        Color32[] pixels = tex.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }
        // Set(pixels, color);
    }
    // public TextureData(Color clr)
    // {
    //     tex = new Texture2D(50, 50);
    //     Set(tex.GetPixels32(), clr);
    // }
    private void Set(Color32[] texPixelColors, Color clr)
    {
        tex = new Texture2D(50, 50);
        color = clr;
        tex.SetPixels32(texPixelColors);
        tex.Apply();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label("", style);
        tex = (Texture2D)EditorGUILayout.ObjectField(tex, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
        tex.Apply();
    }
}