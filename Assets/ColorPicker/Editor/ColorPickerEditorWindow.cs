using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class ColorPickerEditorWindow : EditorWindow
{
    
    Vector2 scrollPos;
    public int submeshIndex;
    public int sharedMatsCount;
    GameObject currentSelected;
    private Color[] clr;
    [SerializeField]private List<TextureData> textures;
    private List<Texture2D> tex;
    private Texture2D atlas;
    private Vector2 scrolPos;
    public Material useMat;
    public static void OpenColorPickerEditorWindow ()
    {
        GetWindow<ColorPickerEditorWindow> ();
    }
    
    
    
    private void OnGUI ()
    {
        

        DisplayControllButtons();
        atlas = EditorGUILayout.ObjectField(atlas, typeof(Texture2D), false) as Texture2D;
        if (textures is null || textures.Count <= 0) return;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        int counter = 0;
        while (counter < textures.Count)
        {
            
            EditorGUILayout.BeginHorizontal();
            for (int k = 0; k < 4; k++)
            {
                EditorGUILayout.BeginVertical();
                textures[counter].color =
                    EditorGUILayout.ColorField(textures[counter].color, GUILayout.Width(50f), GUILayout.Height(30f));
                // textures[i].SetColor();
                GUI.backgroundColor = textures[counter].color;
                MeshFilter filtet = Selection.activeObject.GetComponent<MeshFilter>();
                if (filtet != null)
                {

                    if (filtet.sharedMesh.subMeshCount > 0)
                    {
                        for (int i = 0; i < filtet.sharedMesh.subMeshCount; i++)
                        {
                            if (GUILayout.Button($"Set {counter}", GUILayout.MaxWidth(50f), GUILayout.Height(50f)))
                            {
                                if (Selection.activeGameObject != null)
                                {
                                    ModifyMeshUV(counter, i);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button($"Set {counter}", GUILayout.MaxWidth(50f), GUILayout.Height(50f)))
                        {
                            if (Selection.activeGameObject != null)
                            {
                                ModifyMeshUV(counter);
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
                counter++;
                if (counter > textures.Count - 1)
                {
                    break;
                }
            }
            EditorGUILayout.EndHorizontal();
            
        }
        
        if (GUILayout.Button("Pack", GUILayout.MaxWidth(70)))
        {
            PackTextures(textures.ToArray());
        }
        atlas = (Texture2D)EditorGUILayout.ObjectField(atlas, typeof(Texture2D), false, GUILayout.Width(170), GUILayout.Height(170));
        EditorGUILayout.EndScrollView();
        
    }
    private void ModifyMeshUV(int counter, int submeshIndex_)
    {
        MeshRenderer renderer = Selection.activeGameObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = useMat;
            Material[] submeshMats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                submeshMats[i] = useMat;
            }

            renderer.sharedMaterials = submeshMats;
            MeshFilter mf = Selection.activeGameObject.GetComponent<MeshFilter>();
            SubMeshDescriptor smDc = mf.sharedMesh.GetSubMesh(submeshIndex_);
            List<int> indiciesOfSubmesn = new List<int>();
            mf.sharedMesh.GetIndices(indiciesOfSubmesn, submeshIndex_);
            mf.sharedMesh.GetTriangles(indiciesOfSubmesn, submeshIndex_);
            for (int i = 0; i < indiciesOfSubmesn.Count; i++)
            {
                Debug.Log($"vertices {indiciesOfSubmesn[i]}");
            }
            int endIndex = smDc.vertexCount;
            Vector2[] uv = new Vector2[mf.sharedMesh.uv.Length];
            float pixelValue = RemapValues(0, textures.Count - 1, 0f, 1f, counter);
            Debug.Log($"Pixel value {pixelValue}");
            for (int j = 0; j < uv.Length; j++)
            {
                if (indiciesOfSubmesn.Contains(j))
                {
                    uv[j] = new Vector2(pixelValue, 0f);
                    indiciesOfSubmesn.Remove(j);
                }
                else
                {
                    uv[j] = mf.sharedMesh.uv[j];
                }
            }

            mf.sharedMesh.uv = uv;
            
            renderer.sharedMaterial.mainTexture = atlas;
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
    }
    private void ModifyMeshUV(int counter)
    {
        MeshRenderer renderer = Selection.activeGameObject.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = useMat;
            MeshFilter mf = Selection.activeGameObject.GetComponent<MeshFilter>();
            Vector2[] uv = new Vector2[mf.sharedMesh.uv.Length];
            float pixelValue = RemapValues(0, textures.Count - 1, 0.01f, 0.97f, counter);
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

    private void DisplayControllButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+", GUILayout.MaxWidth(70)))
        {
            if (textures != null)
            {
            }
            else
            {
                textures = new List<TextureData>();
            }

            textures.Add(new TextureData());
        }

        if (GUILayout.Button("-", GUILayout.MaxWidth(70)))
        {
            if (textures != null && textures.Count > 0)
            {
                textures.RemoveAt(textures.Count - 1);
            }
        }

        useMat = EditorGUILayout.ObjectField(useMat, typeof(Material), false) as Material;
        EditorGUILayout.EndHorizontal();
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
    // private void MatToListManagement()
    // {
    //     GUI.backgroundColor = Color.white;
    //     if (GUILayout.Button("+"))
    //     {
    //         Material material = new Material(matList.allMaterials[0]);
    //         AssetDatabase.CreateAsset(material, "Assets/MaterialCustomSelector//BaseMaterials/MyMaterial.mat");
    //         matList.allMaterials.Add(material);
    //     }
    //     if (GUILayout.Button("-"))
    //     {
    //         matList.allMaterials.Remove(matList.allMaterials[matList.allMaterials.Count-1]);
    //     }
    // }
    // private void DisplayMaterialPallette()
    // {
    //     if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<MeshRenderer>() != null)
    //     {
    //         if (currentSelected != Selection.activeGameObject)
    //         {
    //             submeshIndex = 0;
    //             currentSelected = Selection.activeGameObject;
    //         }
    //
    //         MeshRenderer renderer = Selection.activeGameObject.GetComponent<MeshRenderer>();
    //         sharedMatsCount = renderer.sharedMaterials.Length;
    //         if (renderer.sharedMaterials.Length > 1)
    //         {
    //             string btnName = $"materials: {Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterials.Length} | edditing: {submeshIndex}";
    //
    //             if (GUILayout.Button(btnName))
    //             {
    //                 submeshIndex++;
    //                 if (submeshIndex > Selection.activeGameObject.GetComponent<MeshRenderer>().sharedMaterials.Length - 1)
    //                 {
    //                     submeshIndex = 0;
    //                 }
    //             }
    //         }
    //     }
    //     foreach (var item in matList.allMaterials)
    //     {
    //         if (item != null)
    //         {
    //             EditorGUILayout.BeginHorizontal();
    //             GUI.backgroundColor = item.color;
    //
    //             if (GUILayout.Button(item.name, GUILayout.MaxWidth(70)))
    //             {
    //                 ProcessButtonClick(item);
    //             }
    //             var tmp = EditorGUILayout.ObjectField(item, typeof(Material), false, GUILayout.MaxWidth(70));
    //             EditorGUILayout.EndHorizontal(); 
    //         }
    //     }
    // }
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