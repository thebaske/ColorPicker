using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrefabMirrorModule
{
    [CustomEditor(typeof(PrefabMirrorManager))]
    public class PrefabMirroringManagerEditor : Editor
    {
        public string[] allPrefabPaths;
        GameObject[] allPrefabs;
        PrefabMirror[] allMirrors;
        int selectedPath;
        bool loaded;
       
        private void OnEnable()
        {
            loaded = false;
            LoadAll();
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create mirrors"))
            {
                LoadAll();
                SyncPrefabsAndMirrors();
            }
            
            // PrefabMirror scriptable = target as PrefabMirror;
            // if (scriptable != null && loaded)
            // {
            //     base.OnInspectorGUI();
            //
            //     //for (int i = 0; i < allPrefabs.Length; i++)
            //     //{
            //     //    if (!CheckScriptableByName(allScriptables, allPrefabs[i].name))
            //     //    {
            //     //        //create asset and name it. 
            //     //    }
            //     //}
            //     allPrefabPaths = new string[allPrefabs.Length];
            //     for (int i = 0; i < allPrefabPaths.Length; i++)
            //     {
            //         allPrefabPaths[i] = allPrefabs[i].name;
            //     }
            //     scriptable.attachmentPrefabPath = "AttachmentPrefabs/" + allPrefabPaths[GetCurrentlySelectedPath(scriptable)];
            //     scriptable.attachmentPathIndex = EditorGUILayout.Popup(scriptable.attachmentPathIndex, allPrefabPaths);
            //     scriptable.attachmentPrefabPath = "AttachmentPrefabs/" + allPrefabPaths[scriptable.attachmentPathIndex];
            //     string path = AssetDatabase.GetAssetPath(scriptable);
            //     //todo extend so it creates scriptable automatically with guild field and asset path based on that guid, no dropdown or anything. 
            //     //Texture2D tex = PrefabUtility.GetIconForGameObject(Resources.Load<GameObject>(scriptable.attachmentPrefabPath));
            //     scriptable.guid = AssetDatabase.AssetPathToGUID(path);
            //     scriptable.pathByGuid = AssetDatabase.GUIDToAssetPath(scriptable.guid);
            //     Texture2D tex = AssetPreview.GetAssetPreview(Resources.Load<GameObject>(scriptable.attachmentPrefabPath));
            //     TextureField(path, tex);
            //     if (scriptable.name != allPrefabs[scriptable.attachmentPathIndex].name)
            //     {
            //         AssetDatabase.RenameAsset(path, allPrefabs[scriptable.attachmentPathIndex].name);
            //         AssetDatabase.Refresh();
            //         EditorUtility.SetDirty(scriptable); 
            //     }
            // }
        }

        void GetAllMirrors()
        {
            allMirrors = Resources.LoadAll<PrefabMirror>("PrefabMirrors");
        }
        void SyncPrefabsAndMirrors()
        {
            for (int i = 0; i < allPrefabs.Length; i++)
            {
                if (CheckIfMirrorExists(allPrefabs[i].name))
                {
                    
                }
                else
                {
                    CreateMirrorByGameObject(allPrefabs[i]);
                }
            }
        }

        void CreateMirrorByGameObject(GameObject prefab)
        {
            PrefabMirror mirror = ScriptableObject.CreateInstance<PrefabMirror>();
            string path = "Assets/Resources/PrefabMirrors/" + prefab.name;
            AssetDatabase.CreateAsset(mirror, "Assets/Resources/PrefabMirrors/" + prefab.name + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            mirror.resourcesPath = "PrefabMirrors/" + prefab.name;
            string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
            string guidPath = AssetDatabase.GUIDToAssetPath(guid);
            mirror.pathByGuid = guidPath;
            mirror.guid = guid;
            Texture2D tex = AssetPreview.GetAssetPreview(Resources.Load<GameObject>(mirror.pathByGuid));
            TextureField(path, tex);
        }
        bool CheckIfMirrorExists(string name)
        {
            bool found = false;
            if (allMirrors.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < allMirrors.Length; i++)
            {
                if (allMirrors[i].name == name)
                {
                    return true;
                }
            }

            return found;
        }
        int GetCurrentlySelectedPath(PrefabMirror scr)
        {
            int selected = 0;
            for (int i = 0; i < allPrefabPaths.Length; i++)
            {
                if (allPrefabPaths[i] == scr.pathByGuid)
                {
                    selected = i;
                }
            }
            return selected;
        }
        private Texture2D TextureField(string name, Texture2D texture)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;
            style.fixedWidth = 70;
            GUILayout.Label(name, style);
            var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
            GUILayout.EndVertical();
            return result;
        }
        bool CheckScriptableByName(PrefabMirror[] allOfEm, string name)
        {
            bool found = false;
            for (int i = 0; i < allOfEm.Length; i++)
            {
                if (allOfEm[i].name == name)
                {
                    found = true;
                    return found;
                }
            }
            return found;
        }
        void LoadAll()
        {
            allPrefabs = Resources.LoadAll<GameObject>("AttachmentPrefabs");
            GetAllMirrors();
            // allScriptables = Resources.LoadAll<PrefabMirror>("PrefabMorrors");
            loaded = true;
        }
    }
}

