using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
namespace Feedbacks
{
    public class FeedBack_UI_Displayer : MonoBehaviour
    {
        public UnityAction<float, float> OnFeedBackRunning;
        public UnityAction OnFeedBackCompleted;
        [SerializeField] Canvas canvas;
        [SerializeField] Image spriteReceiver;
        [SerializeField] TMP_Text messageTop, messageMiddle, messageBottom;
        [SerializeField] RawImage renderTexture;
        [SerializeField] FeedbackAnimatorHolder stackableAnimator;
        List<FeedbackAnimatorHolder> independantAnimators;
        [SerializeField] RectTransform independantAnimatorsParent;
        Queue<FeedBack> feedBackStack = new Queue<FeedBack>();
        List<FeedBackConstructor> feedBackImmediate = new List<FeedBackConstructor>();
        int immediateFeedbackCounter = 0;
        Camera cam_main;
        string currentState;
        private void OnEnable()
        {
            cam_main = Camera.main;
            independantAnimators = new List<FeedbackAnimatorHolder>();
            for (int i = 0; i < independantAnimatorsParent.childCount; i++)
            {
                independantAnimators.Add(independantAnimatorsParent.GetChild(i).GetComponent<FeedbackAnimatorHolder>());
            }
        }
        public void DisplayFeedbackStackable(FeedBack feedBck)
        {
            if (feedBackStack != null)
            {
                feedBackStack.Enqueue(feedBck);
            }
        }
        public void DisplayFeedbackIndependant(FeedBack feedBck)
        {
            //independantAnimators[immediateFeedbackCounter].gameObject.SetActive(true);
            feedBackImmediate.Add(new FeedBackConstructor(feedBck, independantAnimators[immediateFeedbackCounter]));
            //PushFeedbackImmediate(feedBck, independantAnimators[immediateFeedbackCounter]);

            immediateFeedbackCounter++;
            if (immediateFeedbackCounter > independantAnimators.Count - 1)
            {
                immediateFeedbackCounter = 0;
            }
        }
        bool PushFeedbackImmediate(FeedBack feedBck, FeedbackAnimatorHolder animHolder)
        {
            if (feedBck.durationCounter >= 0)
            {
                animHolder.GetScaleHolder().localScale = new Vector3(feedBck.scale, feedBck.scale, feedBck.scale);
                Vector3 worldPoint = feedBck.worldPosition.position;
                if (feedBck.worldPosition.position != Vector3.zero)
                {
                    worldPoint = cam_main.WorldToScreenPoint(feedBck.worldPosition.position);
                    animHolder.GetScaleHolder().position = worldPoint;
                }
                else
                {
                    animHolder.GetScaleHolder().localPosition = worldPoint;
                }
                animHolder.OnFeedBackRunning?.Invoke(feedBck.duration, feedBck.durationCounter);
                messageMiddle.text = feedBck.message;

                if (SetRenderTexture(feedBck, animHolder))
                {
                    SetTopText(feedBck.message, animHolder);
                }
                else if (SetImage(feedBck, animHolder))
                {
                    SetTopText(feedBck.message, animHolder);
                }
                else
                {
                    SetMiddleText(feedBck.message, animHolder);
                }

                feedBck.durationCounter -= Time.deltaTime;
                if (feedBck.durationCounter <= 0f)
                {
                    animHolder.OnFeedBackRunning?.Invoke(feedBck.duration, feedBck.durationCounter);
                }
                if (feedBck.duration <= animHolder.GetAnimator().GetCurrentAnimatorClipInfo(0).Length)
                {
                    animHolder.GetAnimator().Play(feedBck.feedBackType.ToString(), 0, Utility.RemapValues(feedBck.duration, 0f, 0f, animHolder.GetAnimator().GetCurrentAnimatorClipInfo(0).Length, feedBck.durationCounter));
                }
                else
                {
                    SetCurrentState(feedBck.feedBackType.ToString(), animHolder);
                }
                return true;
            }
            else
            {   
                OnFeedBackCompleted?.Invoke();
                currentState = "";
                return false;
            }

        }
        private void Update()
        {
            FeedBackDriverStackable();
            FeedBackDriverImmediate();
        }
        void FeedBackDriverStackable()
        {
            if (feedBackStack != null && feedBackStack.Count > 0)
            {
                if (!PushFeedbackImmediate(feedBackStack.Peek(), stackableAnimator))
                {
                    feedBackStack.Dequeue();
                    OnFeedBackCompleted?.Invoke();
                    currentState = "";
                }
            }
        }
        void FeedBackDriverImmediate()
        {
            if (feedBackImmediate != null && feedBackImmediate.Count > 0)
            {
                for (int i = 0; i < feedBackImmediate.Count; i++)
                {
                    if (!PushFeedbackImmediate(feedBackImmediate[i].feedback, feedBackImmediate[i].animatorHolder))
                    {
                        feedBackImmediate.RemoveAt(i);
                        OnFeedBackCompleted?.Invoke();
                        currentState = "";
                    } 
                }
            }
        }
        bool SetRenderTexture(FeedBack fdbck, FeedbackAnimatorHolder animHolder)
        {
            if (fdbck.rndTexture != null)
            {
                animHolder.renderTexture.gameObject.SetActive(true);
                animHolder.renderTexture.texture = fdbck.rndTexture;
                return true;
            }
            else
            {
                animHolder.renderTexture.gameObject.SetActive(false);
                return false;
            }
        }
        bool SetImage(FeedBack fdbck, FeedbackAnimatorHolder animHolder)
        {
            if (fdbck.sprite != null)
            {
                animHolder.spriteReceiver.gameObject.SetActive(true);
                animHolder.spriteReceiver.sprite = fdbck.sprite;
                return true;
            }
            else
            {
                animHolder.spriteReceiver.gameObject.SetActive(false);
                return false;
            }
        }
        void SetMiddleText(string text, FeedbackAnimatorHolder animHolder)
        {
            animHolder.messageTop.text = "";
            animHolder.messageBottom.text = "";
            animHolder.messageMiddle.text = text;

        }
        void SetTopText(string text, FeedbackAnimatorHolder animHolder)
        {
            animHolder.messageTop.text = text;
            animHolder.messageBottom.text = "";
            animHolder.messageMiddle.text = "";
        }
        void SetBottomText(string text, FeedbackAnimatorHolder animHolder)
        {
            animHolder.messageBottom.text = text;
            animHolder.messageTop.text = "";
            animHolder.messageMiddle.text = "";
        }
        void SetCurrentState(string state, FeedbackAnimatorHolder animHolder)
        {
            if (currentState != state)
            {
                animHolder.GetAnimator().Play(state, 0, 0f);
                currentState = state;
            }
        }
    }
    public class FeedBackConstructor
    {
        public FeedBack feedback;
        public FeedbackAnimatorHolder animatorHolder;
        public FeedBackConstructor(FeedBack fdb, FeedbackAnimatorHolder fah)
        {
            feedback = fdb;
            animatorHolder = fah;
        }
    }
}
