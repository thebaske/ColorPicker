
using UnityEngine;

namespace PrefabMirrorModule
{
    [CreateAssetMenu(fileName = "PrefabMirror", menuName ="PrefabMirroring/NewPrefabMirror")]
    public class PrefabMirror : ScriptableObject
    {
        public string guid;
        public string pathByGuid;
        public string resourcesPath;
        public bool matched;

        public GameObject Load()
        {
            return Resources.Load<GameObject>(pathByGuid);
        }
    }
}
