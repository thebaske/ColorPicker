using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
namespace Feedbacks
{
    public enum FeedBackTypes { Pop = 0, PopPop = 1, DownDownUp = 2, LeftRightShake = 3 }
    public class FeedBackReceiver : MonoBehaviour
    {
        FeedBack_UI_Displayer ui_Display;
        public FeedbackResources resources;
        [Header("Temporary data")]
        public Sprite testSprite;
        public RenderTexture testRenderTex;
        public static FeedBackReceiver receiver { get; private set; }
        private void Awake()
        {
            receiver = this;
            ui_Display = GetComponent<FeedBack_UI_Displayer>();
            resources = GetComponent<FeedbackResources>();
        }
        public void ReceiveFeedback(string message, float scalePercentage)
        {
            FeedBack fdb = new FeedBack(message, FeedBackTypes.Pop, 0.9f, scalePercentage);
            ui_Display.DisplayFeedbackStackable(fdb);
        }
        [Button("Pop")]
        void Pop()
        {
            ReceiveFeedback("Pop", FeedBackTypes.Pop, 0.1f, 3f);
        }
        [Button("PopPop")]
        void PopPop()
        {
            ReceiveFeedback("PopPop", FeedBackTypes.PopPop, testRenderTex, 3.1f, 0.2f);
        }
        [Button("DownUpDown")]
        void DownUpDown()
        {
            ReceiveFeedback("DownUpDown", FeedBackTypes.DownDownUp, testSprite, 1.5f, 1f);
        }
        public void ReceiveFeedback(string message, FeedBackTypes feedBckType, float duration, float scalePercentage, UnityAction feedBackCallBack = null)
        {
            FeedBack fdb;
            if (feedBackCallBack != null)
            {
                fdb = new FeedBack(message, feedBckType, duration, scalePercentage, feedBackCallBack);
            }
            else
            {
                fdb = new FeedBack(message, feedBckType, duration, scalePercentage);
            }
            ui_Display.DisplayFeedbackStackable(fdb);
        }
        public void ReceiveFeedback(string message, FeedBackTypes feedBckType, Sprite img, float duration, float scalePercentage, UnityAction feedBackCallBack = null)
        {
            FeedBack fdb;
            if (feedBackCallBack != null)
            {
                fdb = new FeedBack(message, feedBckType, img, duration, scalePercentage, feedBackCallBack);
            }
            else
            {
                fdb = new FeedBack(message, feedBckType, img, duration, scalePercentage);
            }
            ui_Display.DisplayFeedbackStackable(fdb);
        }
        public void ReceiveFeedback(string message, FeedBackTypes feedBckType, RenderTexture tex, float duration, float scalePercentage, UnityAction feedBackCallBack = null)
        {
            FeedBack fdb;
            if (feedBackCallBack != null)
            {
                fdb = new FeedBack(message, feedBckType, tex, duration, scalePercentage, feedBackCallBack);
            }
            else
            {
                fdb = new FeedBack(message, feedBckType, tex, duration, scalePercentage);
            }
            ui_Display.DisplayFeedbackStackable(fdb);
        }
        public void ReceiveFeedback(FeedBack fdbck)
        {
            ui_Display.DisplayFeedbackStackable(fdbck);
        }
        public void ReceiveFeedbackImmediate(FeedBack fdbck)
        {
            ui_Display.DisplayFeedbackIndependant(fdbck);
        }
    }
    public class FeedBack
    {
        public string message;
        public FeedBackTypes feedBackType;
        public Sprite sprite;
        public RenderTexture rndTexture;
        public float duration;
        public float durationCounter;
        public float scale;
        public Transform worldPosition;
        public UnityAction callBack;
        public FeedBack()
        {

        }
        public FeedBack(string mssg, FeedBackTypes fdType, float drtion, float scalePercentage)
        {
            message = mssg;
            feedBackType = fdType;
            duration = drtion;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, Sprite img, float drtion, float scalePercentage)
        {
            message = mssg;
            feedBackType = fdType;
            sprite = img;
            duration = drtion;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, RenderTexture rndTx, float drtion, float scalePercentage)
        {
            message = mssg;
            feedBackType = fdType;
            rndTexture = rndTx;
            duration = drtion;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, float drtion, float scalePercentage, UnityAction fdbCllbck)
        {
            message = mssg;
            feedBackType = fdType;
            duration = drtion;
            callBack = fdbCllbck;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, Sprite sprt, float drtion, float scalePercentage, UnityAction fdbCllbck)
        {
            message = mssg;
            feedBackType = fdType;
            sprite = sprt;
            duration = drtion;
            callBack = fdbCllbck;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, RenderTexture rndtx, float drtion, float scalePercentage, UnityAction fdbCllbck)
        {
            message = mssg;
            feedBackType = fdType;
            rndTexture = rndtx;
            duration = drtion;
            callBack = fdbCllbck;
            durationCounter = duration;
            scale = scalePercentage;
        }
        public FeedBack(string mssg, FeedBackTypes fdType, Sprite sprt, float drtion, float scalePercentage, Transform wPos, UnityAction fdbCllbck)
        {
            message = mssg;
            feedBackType = fdType;
            sprite = sprt;
            duration = drtion;
            callBack = fdbCllbck;
            durationCounter = duration;
            scale = scalePercentage;
            worldPosition = wPos;
        }

        
    }
    public struct FeedbackMessage
    {
        string message;
        int position;
    }
}
