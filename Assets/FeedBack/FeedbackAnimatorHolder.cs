using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
namespace Feedbacks
{
    public class FeedbackAnimatorHolder : MonoBehaviour
    {
        public UnityAction<float, float> OnFeedBackRunning;
        [SerializeField] Animator animator;
        [SerializeField] RectTransform scaleHolder;
        public Image spriteReceiver;
        public TMP_Text messageTop, messageMiddle, messageBottom;
        public RawImage renderTexture;
        string currentState;
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            scaleHolder = GetComponent<RectTransform>();
        }
        public Animator GetAnimator()
        {
            return animator;
        }
        public RectTransform GetScaleHolder()
        {
            return scaleHolder;
        }
        
    }
}

