using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Reflection.Emit;
using System;
namespace Feedbacks
{
    public class FeedbackResources : MonoBehaviour
    {

        public Sprite HitMarkers;
        public RenderTexture UnlockedWeapon;

        [SerializeField] public List<FeedbackResource<Sprite>> Sprites = new List<FeedbackResource<Sprite>>();
        [SerializeField] public List<FeedbackResource<RenderTexture>> RenderTextures = new List<FeedbackResource<RenderTexture>>();

        [SerializeField] ReferenceFeedbackHolder referenceHolder = new ReferenceFeedbackHolder();

        private void Awake()
        {
            Sprite[] images = Resources.LoadAll<Sprite>("FeedbackImages");
            ResourceLoader<Sprite> tmpImgs = new ResourceLoader<Sprite>();
            tmpImgs.LoadResources(images, out Sprites);
            RenderTexture[] rt = Resources.LoadAll<RenderTexture>("FeedbackRenderTexture");
            ResourceLoader<RenderTexture> tmpRndTex = new ResourceLoader<RenderTexture>();
            tmpRndTex.LoadResources(rt, out RenderTextures);
        }
        void FillInReferenceHolder()
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                
            }
        }
    }
    public class ReferenceFeedbackHolder
    {
        public Sprite HitMarkers;
        public RenderTexture UnlockedWeapon;
    }
    public class ReferenceRenderTextureHolder
    {
        public string name;
        public RenderTexture sprite;
    }
    public class ResourceLoader<T>
    {
        public void LoadResources(T[] inArray, out List<FeedbackResource<T>> result)
        {
            result = new List<FeedbackResource<T>>();
            
            for (int i = 0; i < inArray.Length; i++)
            {
                Utility.DebugText($"Adding {inArray[i].ToString()}");
                result.Add(new FeedbackResource<T>(inArray[i].ToString(), inArray[i]));
            }
        }
    }
    [System.Serializable]
    public class FeedbackResource<T>
    {
        public string name;
        public T resource;

        public FeedbackResource(string nme, T res)
        {
            name = nme;
            resource = res;
        }
    }
}
